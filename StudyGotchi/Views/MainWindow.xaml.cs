using System.Windows;
using StudyGotchi.Controllers;

namespace StudyGotchi.Views
{
    public partial class MainWindow : Window
    {
        private Views.UserControls.TaskSetupView? _taskSetupView;
        private Views.UserControls.SettingsView? _settingsView;
        private Views.UserControls.SessionSummaryView? _summaryView;
        private Views.StudyWidgetWindow? _widgetWindow;

        public MainWindow()
        {
            InitializeComponent();
            // Initialize app services and switch to ViewModel-driven navigation
            StudyGotchi.Services.ServiceRegistry.Initialize();
            DataContext = new StudyGotchi.ViewModels.MainWindowViewModel();
        }

        public void NavigateToPetSelection()
        {
            var vm = DataContext as StudyGotchi.ViewModels.MainWindowViewModel;
            if (vm != null)
            {
                vm.CurrentViewModel = StudyGotchi.Services.ServiceRegistry.PetSelectionViewModel;
            }
        }

        public void NavigateToTaskSetup()
        {
            var vm = DataContext as StudyGotchi.ViewModels.MainWindowViewModel;
            if (vm != null)
            {
                vm.CurrentViewModel = _taskSetupView ??= new Views.UserControls.TaskSetupView();
            }
        }

        public void NavigateToSettings()
        {
            var vm = DataContext as StudyGotchi.ViewModels.MainWindowViewModel;
            if (vm != null)
            {
                vm.CurrentViewModel = _settingsView ??= new Views.UserControls.SettingsView();
            }
        }

        public void NavigateToDashboard()
        {
            var vm = DataContext as StudyGotchi.ViewModels.MainWindowViewModel;
            if (vm != null)
            {
                StudyGotchi.Services.ServiceRegistry.DashboardViewModel.Refresh();
                vm.CurrentViewModel = StudyGotchi.Services.ServiceRegistry.DashboardViewModel;
            }
        }

        public void NavigateToSummary()
        {
            var vm = DataContext as StudyGotchi.ViewModels.MainWindowViewModel;
            if (vm != null)
            {
                vm.CurrentViewModel = _summaryView ??= new Views.UserControls.SessionSummaryView();
            }
        }

        public void ShowStarvationAlert()
        {
            MessageBox.Show("Pet is starving! XP gain reduced.", "Warning");
        }

        public void LaunchWidgetMode()
        {
            // if a widget is already open, bring it to front
            if (_widgetWindow != null && _widgetWindow.IsVisible)
            {
                _widgetWindow.Activate();
                return;
            }

            // create and show widget; hide main window
            _widgetWindow = new StudyWidgetWindow();
            _widgetWindow.Owner = this;
            // when widget closes, show main window and navigate to dashboard
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