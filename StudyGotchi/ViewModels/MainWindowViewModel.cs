namespace StudyGotchi.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private object? _currentViewModel;

        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; RaisePropertyChanged(); }
        }

        public MainWindowViewModel()
        {
            CurrentViewModel = StudyGotchi.Services.ServiceRegistry.PetSelectionViewModel;
        }
    }
}
