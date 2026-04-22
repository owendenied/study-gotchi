using System.Windows;
using StudyGotchi.Controllers;

namespace StudyGotchi.Views
{
    public partial class MainWindow : Window
    {
        private SessionController _sessionController;
        private Views.UserControls.PetSelectionView _petSelectionView;
        private Views.UserControls.TaskSetupView _taskSetupView;
        private Views.UserControls.SettingsView _settingsView;
        private Views.UserControls.DashboardView _dashboardView;
        private Views.UserControls.SessionSummaryView _summaryView;
        private Views.StudyWidgetWindow? _widgetWindow;

        public MainWindow()
        {
            InitializeComponent();
            // switch to ViewModel-driven navigation
            DataContext = new StudyGotchi.ViewModels.MainWindowViewModel();
        }

        public void NavigateToPetSelection()
        {
            var vm = DataContext as StudyGotchi.ViewModels.MainWindowViewModel;
            if (vm != null)
            {
                vm.CurrentViewModel = _petSelectionView ??= new Views.UserControls.PetSelectionView();
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
                vm.CurrentViewModel = _dashboardView ??= new Views.UserControls.DashboardView();
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