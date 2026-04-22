using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StudyGotchi.ViewModels;

namespace StudyGotchi.ViewModels
{
    public class SessionViewModel : BaseViewModel
    {
        private bool _isSessionActive;

        public bool IsSessionActive
        {
            get => _isSessionActive;
            set { _isSessionActive = value; RaisePropertyChanged(); }
        }

        public ICommand StartCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand EndCommand { get; }

        public SessionViewModel()
        {
            StartCommand = new RelayCommand(_ => StartSession());
            PauseCommand = new RelayCommand(_ => PauseSession());
            EndCommand = new RelayCommand(_ => EndSession());
        }

        private void StartSession()
        {
            IsSessionActive = true;
        }

        private void PauseSession()
        {
            IsSessionActive = false;
        }

        private void EndSession()
        {
            IsSessionActive = false;
            var wnd = Application.Current?.Windows.OfType<StudyGotchi.Views.MainWindow>().FirstOrDefault();
            wnd?.NavigateToSummary();
        }
    }
}
