using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StudyGotchi.Controllers;

namespace StudyGotchi.ViewModels
{
    public class SessionViewModel : BaseViewModel
    {
        private SessionController _sessionController;
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

        public ICommand StartCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand EndCommand { get; }

        public SessionViewModel(SessionController sessionController)
        {
            _sessionController = sessionController;
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
            };

            StartCommand = new RelayCommand(_ => StartSession());
            PauseCommand = new RelayCommand(_ => TogglePause());
            EndCommand = new RelayCommand(_ => EndSession());

            _sessionController.PauseStateChanged += paused => IsPaused = paused;
        }

        private void StartSession()
        {
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
            
            // Refresh summary stats for the view
            RaisePropertyChanged(nameof(TasksDone));
            RaisePropertyChanged(nameof(XpEarned));
            RaisePropertyChanged(nameof(CurrentStage));

            var wnd = Application.Current?.Windows.OfType<StudyGotchi.Views.MainWindow>().FirstOrDefault();
            wnd?.NavigateToSummary();
        }
    }
}
