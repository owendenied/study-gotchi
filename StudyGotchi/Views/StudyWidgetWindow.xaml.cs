using System.Windows;
using System.Windows.Threading;
using StudyGotchi.Controllers;

namespace StudyGotchi.Views
{
    public partial class StudyWidgetWindow : Window
    {
        private DispatcherTimer? _toastTimer;

        public StudyWidgetWindow()
        {
            InitializeComponent();
            this.DataContext = StudyGotchi.Services.ServiceRegistry.DashboardViewModel;

            // Subscribe to reminder events so the widget can show toast notifications
            var reminderService = StudyGotchi.Services.ServiceRegistry.ReminderService;
            if (reminderService != null)
                reminderService.ReminderTriggered += OnReminderTriggered;

            // Subscribe to audio service events (if any future direct hooks needed)
        }

        private void OnReminderTriggered(string taskName, int minutesLeft)
        {
            // Always dispatch to UI thread since the reminder timer runs on the UI thread
            // but guard just in case
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                ShowToast($"{taskName}\n{minutesLeft} min left!");
                StudyGotchi.Services.ServiceRegistry.AudioService?.PlaySfx("sfx_reminder");
            });
        }

        public void ShowToast(string message)
        {
            TxtToastMessage.Text = message;
            ToastBorder.Visibility = Visibility.Visible;

            // Reset any running auto-hide timer
            _toastTimer?.Stop();
            _toastTimer = new DispatcherTimer { Interval = System.TimeSpan.FromSeconds(4) };
            _toastTimer.Tick += (s, e) =>
            {
                _toastTimer?.Stop();
                ToastBorder.Visibility = Visibility.Collapsed;
            };
            _toastTimer.Start();
        }

        public void UpdatePetDisplay()
        {
            // placeholder to refresh visual state from controllers
        }

        public void ReturnToDashboard()
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                // double-click detected: show main window (dashboard) and close widget
                var main = Application.Current?.Windows.OfType<MainWindow>().FirstOrDefault();
                if (main != null)
                {
                    main.Show();
                    main.NavigateToDashboard();
                }
                this.Close();
            }
            else
            {
                // start drag move on single click
                try { this.DragMove(); } catch { }
            }
        }
    }
}