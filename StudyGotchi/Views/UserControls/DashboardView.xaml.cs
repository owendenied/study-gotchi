using System.Windows.Controls;

namespace StudyGotchi.Views.UserControls
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            DataContext = StudyGotchi.Services.ServiceRegistry.DashboardViewModel;
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

        private void TaskCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null) return;

            cb.IsEnabled = false;

            if (!StudyGotchi.Services.ServiceRegistry.SessionViewModel.IsSessionActive)
            {
                cb.IsChecked = false;
                cb.IsEnabled = true;
                var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
                wnd?.ShowToast("Start a session before completing tasks.");
                return;
            }

            if (StudyGotchi.Services.ServiceRegistry.SessionViewModel.IsPaused)
            {
                cb.IsChecked = false;
                cb.IsEnabled = true;
                var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
                wnd?.ShowToast("Resume the session before completing tasks.");
                return;
            }

            if (cb.Tag is int taskId)
            {
                StudyGotchi.Services.ServiceRegistry.TaskController.CompleteTask(taskId);
            }
            else
            {
                cb.IsChecked = false;
                cb.IsEnabled = true;
            }
        }

    }
}
