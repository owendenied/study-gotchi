using System.Windows.Controls;

namespace StudyGotchi.Views.UserControls
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            this.DataContext = StudyGotchi.Services.ServiceRegistry.DashboardViewModel;
        }

        private void BtnStartSession_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StudyGotchi.Services.ServiceRegistry.SessionViewModel.StartCommand.Execute(null);
        }

        private void BtnPause_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StudyGotchi.Services.ServiceRegistry.SessionViewModel.PauseCommand.Execute(null);
        }

        private void BtnEndSession_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StudyGotchi.Services.ServiceRegistry.SessionViewModel.EndCommand.Execute(null);
        }

        private void BtnAddTaskTop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToTaskSetup();
        }

        private void BtnNavSelectPet_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToPetSelection();
        }

        private void BtnNavSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToSettings();
        }

        private void BtnWidgetMode_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.LaunchWidgetMode();
        }

        private void BtnMuteToggle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Primitives.ToggleButton;
            if (btn == null) return;

            var audioSvc = StudyGotchi.Services.ServiceRegistry.AudioService;
            if (audioSvc != null)
            {
                audioSvc.IsMuted = btn.IsChecked == true;
                btn.Content = audioSvc.IsMuted ? "🔇" : "🔊";
            }
        }

        private void BtnFullScreen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.ToggleFullScreen();
        }

        private void TaskCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var cb = sender as System.Windows.Controls.CheckBox;
            if (cb == null) return;

            // Immediately lock the whole row so the user can't click again while it fades
            var row = cb.Parent as System.Windows.Controls.StackPanel;
            if (row != null) row.IsHitTestVisible = false;

            if (!StudyGotchi.Services.ServiceRegistry.SessionViewModel.IsSessionActive)
            {
                cb.IsChecked = false;
                if (row != null) row.IsHitTestVisible = true; // re-enable, session not active
                System.Windows.MessageBox.Show("Please start a session before completing tasks!", "Session Not Active", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            if (cb.Tag is int taskId)
            {
                var tc = StudyGotchi.Services.ServiceRegistry.TaskController;
                tc.CompleteTask(taskId);
            }
        }

        public void OnTaskChecked()
        {
            // UML contract — reserved for future implementation
        }

        public void UpdateDisplay()
        {
            // UML contract — reserved for future implementation
        }
    }
}