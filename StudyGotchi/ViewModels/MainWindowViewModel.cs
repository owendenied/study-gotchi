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
            // use shared services
            var tc = StudyGotchi.Services.ServiceRegistry.TaskController;
            var tasksVm = StudyGotchi.Services.ServiceRegistry.TasksViewModel;

            ShowPetSelectionCommand = new RelayCommand(_ => CurrentViewModel = StudyGotchi.Services.ServiceRegistry.PetSelectionViewModel);
            ShowTaskSetupCommand = new RelayCommand(_ => CurrentViewModel = new Views.UserControls.TaskSetupView());
            ShowSettingsCommand = new RelayCommand(_ => CurrentViewModel = new Views.UserControls.SettingsView());
            ShowDashboardCommand = new RelayCommand(_ => CurrentViewModel = StudyGotchi.Services.ServiceRegistry.DashboardViewModel);
            ShowSummaryCommand = new RelayCommand(_ => CurrentViewModel = new Views.UserControls.SessionSummaryView());

            // default
            CurrentViewModel = StudyGotchi.Services.ServiceRegistry.PetSelectionViewModel;
        }
    }
}
