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
            UpdateSessionButtons();
        }

        private void BtnPause_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StudyGotchi.Services.ServiceRegistry.SessionViewModel.PauseCommand.Execute(null);
            UpdateSessionButtons();
        }

        private void UpdateSessionButtons()
        {
            // The bindings in XAML should handle visibility automatically now
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

        private void TaskCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!StudyGotchi.Services.ServiceRegistry.SessionViewModel.IsSessionActive)
            {
                if (sender is CheckBox cb) cb.IsChecked = false;
                System.Windows.MessageBox.Show("Please start a session before completing tasks!", "Session Not Active", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            if (sender is CheckBox cb2 && cb2.Tag is int taskId)
            {
                var tc = StudyGotchi.Services.ServiceRegistry.TaskController;
                tc.CompleteTask(taskId);
            }
        }

        public void OnTaskChecked()
        {
            // placeholder for later
        }

        public void UpdateDisplay()
        {
            // placeholder for later
        }

        private void ListBoxItem_Selected(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}