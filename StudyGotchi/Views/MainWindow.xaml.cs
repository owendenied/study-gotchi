using System.Windows;

namespace StudyGotchi.Views
{
    public partial class MainWindow : Window
    {
        private Views.UserControls.TaskSetupView? _taskSetupView;
        private Views.UserControls.SettingsView? _settingsView;
        private Views.UserControls.SessionSummaryView? _summaryView;
        private Views.StudyWidgetWindow? _widgetWindow;
        private bool _isFullScreen = false;
        private System.Windows.Threading.DispatcherTimer? _toastTimer;

        private ViewModels.MainWindowViewModel? ViewModel => DataContext as ViewModels.MainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            Services.ServiceRegistry.Initialize();
            DataContext = new ViewModels.MainWindowViewModel();

            Loaded += (s, e) =>
            {
                if (Services.ServiceRegistry.IsFullScreenPreferred && !_isFullScreen)
                {
                    ToggleFullScreen();
                }
            };

            // Subscribe to reminders for the main window toast
            Services.ServiceRegistry.ReminderService.ReminderTriggered += OnReminderTriggered;
            Services.ServiceRegistry.PetController.PetLeveledUp += () =>
            {
                Application.Current?.Dispatcher?.Invoke(() => ShowToast("Your pet evolved a little.", "LEVEL UP", "+"));
            };
        }

        private void OnReminderTriggered(string taskName, int minutesLeft)
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                ShowToast($"{taskName} ({minutesLeft} min left!)", "REMINDER", "!");
            });
        }

        public void ShowToast(string message, string title = "NOTICE", string icon = "!")
        {
            TxtToastTitle.Text = title;
            TxtToastIcon.Text = icon;
            TxtToastMessage.Text = message;
            ToastBorder.Visibility = Visibility.Visible;

            _toastTimer?.Stop();
            _toastTimer = new System.Windows.Threading.DispatcherTimer { Interval = System.TimeSpan.FromSeconds(5) };
            _toastTimer.Tick += (s, e) =>
            {
                _toastTimer?.Stop();
                ToastBorder.Visibility = Visibility.Collapsed;
            };
            _toastTimer.Start();
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

        public void ToggleFullScreen()
        {
            if (_isFullScreen)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
                _isFullScreen = false;
                Services.ServiceRegistry.IsFullScreenPreferred = false;
            }
            else
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                _isFullScreen = true;
                Services.ServiceRegistry.IsFullScreenPreferred = true;
            }

            Services.ServiceRegistry.SaveState();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F11)
            {
                ToggleFullScreen();
            }
            else if (e.Key == System.Windows.Input.Key.Escape && _isFullScreen)
            {
                ToggleFullScreen();
            }
        }
    }
}
