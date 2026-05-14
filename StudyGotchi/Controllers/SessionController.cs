using System;
using System.Windows.Threading;
using StudyGotchi.Models;

namespace StudyGotchi.Controllers
{
    public class SessionController
    {
        private PetController _petController;
        private TaskController _taskController;
        private bool _sessionActive;
        private bool _isPaused;
        private DispatcherTimer _sessionTimer;
        private DateTime _startTime;
        private TimeSpan _elapsedTime;
        private TimeSpan _pausedOffset;   // accumulates time already elapsed before a pause
        private int _tasksCompletedThisSession;
        private int _xpEarnedThisSession;
        private int _startHunger;
        private int _startLevel;
        private int _endHunger;
        private int _endLevel;
        private TimeSpan _lastSessionDuration;

        public event Action<string>? TimeUpdated;
        public event Action? StatsUpdated;
        public event Action<bool>? PauseStateChanged;  // true = paused
        public event Action? SessionStarted;
        public event Action? SessionEnded;
        public event Action? SessionReset;

        public int TasksCompletedThisSession => _tasksCompletedThisSession;
        public int XpEarnedThisSession => _xpEarnedThisSession;
        public bool IsPaused => _isPaused;
        public TimeSpan LastSessionDuration => _lastSessionDuration;
        public TimeSpan CurrentElapsedTime
        {
            get
            {
                if (!_sessionActive) return _lastSessionDuration;
                return _isPaused ? _elapsedTime : _pausedOffset + (DateTime.Now - _startTime);
            }
        }
        public int StartHunger => _startHunger;
        public int StartLevel => _startLevel;
        public int EndHunger => _endHunger;
        public int EndLevel => _endLevel;
        public int HungerChange => _endHunger - _startHunger;
        public int LevelChange => _endLevel - _startLevel;
        public string LastSessionDurationText => _lastSessionDuration.ToString(@"hh\:mm\:ss");
        public string HungerChangeText => HungerChange >= 0 ? $"+{HungerChange}" : HungerChange.ToString();
        public string LevelChangeText => LevelChange >= 0 ? $"+{LevelChange}" : LevelChange.ToString();
        public string FeedbackText
        {
            get
            {
                if (_tasksCompletedThisSession >= 3) return "Big focus run. Your pet felt that.";
                if (_tasksCompletedThisSession > 0) return "Nice session. Small steps still feed the streak.";
                return "Session logged. Add a task next time for rewards.";
            }
        }

        public SessionController(PetController petController, TaskController taskController)
        {
            _petController = petController;
            _taskController = taskController;
            _sessionTimer = new DispatcherTimer();
            _sessionTimer.Interval = TimeSpan.FromSeconds(1);
            _sessionTimer.Tick += OnTimerTick;

            _taskController.TaskCompleted += (t) => 
            {
                if (_sessionActive)
                {
                    _tasksCompletedThisSession++;
                    _xpEarnedThisSession += EstimateTaskXp(t);
                }
            };
        }

        public void StartSession() 
        { 
            _sessionActive = true;
            _isPaused = false;
            _startTime = DateTime.Now;
            _elapsedTime = TimeSpan.Zero;
            _pausedOffset = TimeSpan.Zero;
            _tasksCompletedThisSession = 0;
            _xpEarnedThisSession = 0;
            _startHunger = _petController.GetHungerLevel();
            _startLevel = _petController.GetLevel();
            _endHunger = _startHunger;
            _endLevel = _startLevel;
            _lastSessionDuration = TimeSpan.Zero;
            _sessionTimer.Start();
            TimeUpdated?.Invoke("00:00:00");
            SessionStarted?.Invoke();
        }

        public void PauseSession()
        {
            if (!_sessionActive || _isPaused) return;
            _elapsedTime = _pausedOffset + (DateTime.Now - _startTime);
            _isPaused = true;
            _pausedOffset = _elapsedTime;  // save current elapsed so we resume from here
            _sessionTimer.Stop();
            PauseStateChanged?.Invoke(true);
        }

        public void ResumeSession()
        {
            if (!_sessionActive || !_isPaused) return;
            _isPaused = false;
            _startTime = DateTime.Now;     // reset start to now; offset accounts for previous time
            _sessionTimer.Start();
            PauseStateChanged?.Invoke(false);
        }

        public void EndSession()
        {
            EndSession(recordCompletion: true);
        }

        public void EndSession(bool recordCompletion) 
        { 
            if (!_sessionActive) return;

            if (!_isPaused)
            {
                _elapsedTime = _pausedOffset + (DateTime.Now - _startTime);
            }

            _lastSessionDuration = _elapsedTime;
            _endHunger = _petController.GetHungerLevel();
            _endLevel = _petController.GetLevel();
            _sessionActive = false;
            _isPaused = false;
            _sessionTimer.Stop();
            if (recordCompletion)
            {
                SessionEnded?.Invoke();
            }
            else
            {
                SessionReset?.Invoke();
            }
        }

        public void ResetSession()
        {
            EndSession(recordCompletion: false);
            _elapsedTime = TimeSpan.Zero;
            _pausedOffset = TimeSpan.Zero;
            _tasksCompletedThisSession = 0;
            _xpEarnedThisSession = 0;
            _lastSessionDuration = TimeSpan.Zero;
        }

        public void RestoreSession(bool isSessionActive, bool isPaused, TimeSpan elapsedTime, int tasksCompleted, int xpEarned, int startHunger, int startLevel, int endHunger, int endLevel, TimeSpan lastSessionDuration)
        {
            _sessionActive = isSessionActive;
            _isPaused = isPaused;
            _elapsedTime = elapsedTime;
            _pausedOffset = elapsedTime;
            _tasksCompletedThisSession = tasksCompleted;
            _xpEarnedThisSession = xpEarned;
            _startHunger = startHunger;
            _startLevel = startLevel;
            _endHunger = endHunger;
            _endLevel = endLevel;
            _lastSessionDuration = lastSessionDuration;

            _sessionTimer.Stop();

            if (_sessionActive && !_isPaused)
            {
                _startTime = DateTime.Now;
                _sessionTimer.Start();
                TimeUpdated?.Invoke(_elapsedTime.ToString(@"hh\:mm\:ss"));
                SessionStarted?.Invoke();
            }
            else if (_sessionActive && _isPaused)
            {
                TimeUpdated?.Invoke(_elapsedTime.ToString(@"hh\:mm\:ss"));
                PauseStateChanged?.Invoke(true);
            }
        }

        public bool IsSessionActive() { return _sessionActive; }

        public int HungerDecayRate { get; set; } = 2;

        private void OnTimerTick(object? sender, EventArgs e)
        {
            _elapsedTime = _pausedOffset + (DateTime.Now - _startTime);
            TimeUpdated?.Invoke(_elapsedTime.ToString(@"hh\:mm\:ss"));
            
            if ((int)_elapsedTime.TotalSeconds % 15 == 0 && (int)_elapsedTime.TotalSeconds > 0)
            {
                int decayAmount = HungerDecayRate switch {
                    1 => 1,  // Low
                    2 => 2,  // Medium
                    3 => 4,  // High
                    _ => 2
                };
                _petController.GetActivePet()?.DecayHunger(decayAmount);
                StatsUpdated?.Invoke();
            }
        }

        public string GetSessionSummary()
        {
            return _sessionActive ? "Session is currently running..." : $"Session ended. Tasks completed: {_tasksCompletedThisSession}, XP earned: {_xpEarnedThisSession}";
        }

        public int EstimateTaskXp(StudyTask task)
        {
            return _petController.GetActivePet()?.CalculateTaskXp(task.IsCompletedEarly, task.BaseXpReward)
                ?? task.BaseXpReward;
        }
    }
}
