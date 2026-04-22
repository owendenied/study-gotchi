using System.Windows.Controls;

namespace StudyGotchi.Views.UserControls
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
        }

        private readonly StudyGotchi.ViewModels.SessionViewModel _sessionVm = new StudyGotchi.ViewModels.SessionViewModel();

        private void BtnStartSession_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // start session and update UI: replace button with pause/end
            _sessionVm.StartCommand.Execute(null);
            UpdateSessionButtons();
        }

        private void BtnPause_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _sessionVm.PauseCommand.Execute(null);
            UpdateSessionButtons();
        }

        private void UpdateSessionButtons()
        {
            if (_sessionVm.IsSessionActive)
            {
                // remove Start button and show Pause/End
                StartSessionPlaceholder.Visibility = System.Windows.Visibility.Collapsed;
                SessionControlsPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                StartSessionPlaceholder.Visibility = System.Windows.Visibility.Visible;
                SessionControlsPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void BtnEndSession_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToSummary();
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