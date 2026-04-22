using StudyGotchi.ViewModels;
using System.Windows.Input;

namespace StudyGotchi.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; RaisePropertyChanged(); }
        }

        public ICommand ShowPetSelectionCommand { get; }
        public ICommand ShowTaskSetupCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowSummaryCommand { get; }

        public MainWindowViewModel()
        {
            ShowPetSelectionCommand = new RelayCommand(_ => CurrentViewModel = new Views.UserControls.PetSelectionView());
            ShowTaskSetupCommand = new RelayCommand(_ => CurrentViewModel = new Views.UserControls.TaskSetupView());
            ShowSettingsCommand = new RelayCommand(_ => CurrentViewModel = new Views.UserControls.SettingsView());
            ShowDashboardCommand = new RelayCommand(_ => CurrentViewModel = new Views.UserControls.DashboardView());
            ShowSummaryCommand = new RelayCommand(_ => CurrentViewModel = new Views.UserControls.SessionSummaryView());

            // default
            CurrentViewModel = new Views.UserControls.PetSelectionView();
        }
    }
}
