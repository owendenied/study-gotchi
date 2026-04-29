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
        private bool _isFocusMode;

        public event Action<string>? TimeUpdated;
        public event Action? StatsUpdated;
        public event Action<bool>? PauseStateChanged;  // true = paused

        public int TasksCompletedThisSession => _tasksCompletedThisSession;
        public int XpEarnedThisSession => _xpEarnedThisSession;
        public bool IsPaused => _isPaused;

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
                    _xpEarnedThisSession += 25; // Matching the reward in TamagotchiPet
                }
            };
        }

        public void StartSession()
        {
            StartSession(false);
        }

        public void StartSession(bool focusMode) 
        { 
            _sessionActive = true;
            _isPaused = false;
            _isFocusMode = focusMode;
            _startTime = DateTime.Now;
            _elapsedTime = TimeSpan.Zero;
            _pausedOffset = TimeSpan.Zero;
            _tasksCompletedThisSession = 0;
            _xpEarnedThisSession = 0;
            _sessionTimer.Start();
            TimeUpdated?.Invoke("00:00:00");
        }

        public void PauseSession()
        {
            if (!_sessionActive || _isPaused) return;
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
            _sessionActive = false;
            _sessionTimer.Stop();
        }

        public bool IsSessionActive() { return _sessionActive; }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            _elapsedTime = _pausedOffset + (DateTime.Now - _startTime);
            TimeUpdated?.Invoke(_elapsedTime.ToString(@"hh\:mm\:ss"));
            
            // Moderate hunger decay: decay every 15 seconds
            if ((int)_elapsedTime.TotalSeconds % 15 == 0 && (int)_elapsedTime.TotalSeconds > 0)
            {
                int decayAmount = _isFocusMode ? 4 : 2;
                _petController.GetActivePet()?.DecayHunger(decayAmount);
                StatsUpdated?.Invoke();
            }
        }

        public string GetSessionSummary()
        {
            return _sessionActive ? "Session is currently running..." : $"Session ended. Tasks completed: {_tasksCompletedThisSession}, XP earned: {_xpEarnedThisSession}";
        }
    }
}