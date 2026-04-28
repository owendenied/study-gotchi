using System.Windows;
using StudyGotchi.Controllers;

namespace StudyGotchi.Views
{
    public partial class MainWindow : Window
    {
        private SessionController _sessionController;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetFocusMode(bool isEnabled)
        {
            this.Topmost = isEnabled;
        }

        public void NavigateToPetSelection()
        {
            // Logic: Hide the current view and show the Pet Selection screen
        }

        public void LaunchWidgetMode()
        {
            // Logic: Shrink the window to a small "Gotchi" size
            this.WindowState = WindowState.Normal;
            this.Width = 300;
            this.Height = 400;
        }

        public void ShowStarvationAlert()
        {
            MessageBox.Show("Pet is starving! XP gain reduced.", "Warning");
        }
    }
}