using System.Windows;

namespace StudyGotchi.Views
{
    public partial class MainWindow : Window
    {
        private Views.UserControls.TaskSetupView? _taskSetupView;
        private Views.UserControls.SettingsView? _settingsView;
        private Views.UserControls.SessionSummaryView? _summaryView;
        private Views.StudyWidgetWindow? _widgetWindow;

        private ViewModels.MainWindowViewModel? ViewModel => DataContext as ViewModels.MainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            Services.ServiceRegistry.Initialize();
            DataContext = new ViewModels.MainWindowViewModel();
        }

        public void NavigateToPetSelection()
        {
            if (ViewModel != null)
                ViewModel.CurrentViewModel = Services.ServiceRegistry.PetSelectionViewModel;
        }

        public void NavigateToTaskSetup()
        {
            if (ViewModel != null)
                ViewModel.CurrentViewModel = _taskSetupView ??= new Views.UserControls.TaskSetupView();
        }

        public void NavigateToSettings()
        {
            if (ViewModel != null)
                ViewModel.CurrentViewModel = _settingsView ??= new Views.UserControls.SettingsView();
        }

        public void NavigateToDashboard()
        {
            if (ViewModel != null)
            {
                Services.ServiceRegistry.DashboardViewModel.Refresh();
                ViewModel.CurrentViewModel = Services.ServiceRegistry.DashboardViewModel;
            }
        }

        public void NavigateToSummary()
        {
            if (ViewModel != null)
                ViewModel.CurrentViewModel = _summaryView ??= new Views.UserControls.SessionSummaryView();
        }

        public void ShowStarvationAlert()
        {
            MessageBox.Show("Pet is starving! XP gain reduced.", "Warning");
        }

        public void LaunchWidgetMode()
        {
            if (_widgetWindow != null && _widgetWindow.IsVisible)
            {
                _widgetWindow.Activate();
                return;
            }

            _widgetWindow = new StudyWidgetWindow();
            _widgetWindow.Owner = this;
            _widgetWindow.Closed += (s, e) =>
            {
                this.Show();
                NavigateToDashboard();
                _widgetWindow = null;
            };

            this.Hide();
            _widgetWindow.Show();
        }
    }
}