using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StudyGotchi.Controllers;

namespace StudyGotchi.ViewModels
{
    public class SessionViewModel : BaseViewModel
    {
        private readonly SessionController _sessionController;
        private bool _isSessionActive;
        private bool _isPaused;
        private string _clockText = "No session active";

        public bool IsSessionActive
        {
            get => _isSessionActive;
            set { _isSessionActive = value; RaisePropertyChanged(); }
        }

        public bool IsPaused
        {
            get => _isPaused;
            set { _isPaused = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(PauseButtonText)); }
        }

        public string PauseButtonText => _isPaused ? "Resume ▶" : "Pause ⏸";

        public string ClockText
        {
            get => _clockText;
            set { _clockText = value; RaisePropertyChanged(); }
        }

        public string TasksDone => _sessionController.TasksCompletedThisSession.ToString();
        public string XpEarned => _sessionController.XpEarnedThisSession.ToString();
        public string CurrentStage => StudyGotchi.Services.ServiceRegistry.PetController.GetEvolutionStageName();
        public string Duration => _sessionController.LastSessionDurationText;
        public string HungerChange => _sessionController.HungerChangeText;
        public string LevelChange => _sessionController.LevelChangeText;
        public string FeedbackText => _sessionController.FeedbackText;
        public string TotalSessions => StudyGotchi.Services.ServiceRegistry.TotalSessionsCompleted.ToString();

        public ICommand StartCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand EndCommand { get; }

        public SessionViewModel(SessionController sessionController)
        {
            _sessionController = sessionController;
            IsSessionActive = _sessionController.IsSessionActive();
            IsPaused = _sessionController.IsPaused;
            ClockText = _sessionController.IsSessionActive()
                ? _sessionController.CurrentElapsedTime.ToString(@"hh\:mm\:ss")
                : "No session active";
            _sessionController.TimeUpdated += (t) => ClockText = t;
            _sessionController.SessionStarted += () => 
            {
                IsSessionActive = true;
                IsPaused = false;
            };
            _sessionController.SessionEnded += () => 
            {
                IsSessionActive = false;
                ClockText = "No session active";
                RaiseSummaryProperties();
            };

            StartCommand = new RelayCommand(_ => StartSession());
            PauseCommand = new RelayCommand(_ => TogglePause());
            EndCommand = new RelayCommand(_ => EndSession());

            _sessionController.PauseStateChanged += paused => IsPaused = paused;
        }

        private void StartSession()
        {
            if (!StudyGotchi.Services.ServiceRegistry.PetController.HasActivePet())
            {
                ClockText = "Choose a pet first";
                return;
            }

            if (!StudyGotchi.Services.ServiceRegistry.TaskController.HasTasks())
            {
                ClockText = "Add a task to earn XP";
                return;
            }

            IsSessionActive = true;
            IsPaused = false;
            _sessionController.StartSession();
        }

        private void TogglePause()
        {
            if (!_sessionController.IsPaused)
                _sessionController.PauseSession();
            else
                _sessionController.ResumeSession();
        }

        private void EndSession()
        {
            IsSessionActive = false;
            _sessionController.EndSession();
            ClockText = "No session active";
            
            RaiseSummaryProperties();

            var wnd = Application.Current?.Windows.OfType<StudyGotchi.Views.MainWindow>().FirstOrDefault();
            wnd?.NavigateToSummary();
        }

        public void RefreshSummary()
        {
            RaiseSummaryProperties();
        }

        private void RaiseSummaryProperties()
        {
            RaisePropertyChanged(nameof(TasksDone));
            RaisePropertyChanged(nameof(XpEarned));
            RaisePropertyChanged(nameof(CurrentStage));
            RaisePropertyChanged(nameof(Duration));
            RaisePropertyChanged(nameof(HungerChange));
            RaisePropertyChanged(nameof(LevelChange));
            RaisePropertyChanged(nameof(FeedbackText));
            RaisePropertyChanged(nameof(TotalSessions));
        }
    }
}
