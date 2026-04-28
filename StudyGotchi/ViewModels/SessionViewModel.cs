using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StudyGotchi.ViewModels;
using StudyGotchi.Controllers;

namespace StudyGotchi.ViewModels
{
    public class SessionViewModel : BaseViewModel
    {
        private SessionController _sessionController;
        private bool _isSessionActive;
        private string _clockText = "No session active";

        public bool IsSessionActive
        {
            get => _isSessionActive;
            set { _isSessionActive = value; RaisePropertyChanged(); }
        }

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

            StartCommand = new RelayCommand(_ => StartSession());
            PauseCommand = new RelayCommand(_ => PauseSession());
            EndCommand = new RelayCommand(_ => EndSession());
        }

        private void StartSession()
        {
            IsSessionActive = true;
            _sessionController.StartSession();
        }

        private void PauseSession()
        {
            // Placeholder for pause logic if needed
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
