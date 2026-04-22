using System.Windows.Controls;

namespace StudyGotchi.Views.UserControls
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToTaskSetup();
        }

        private void BtnLetsGo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Save pet name
            var name = TxtPetName.Text?.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                var pc = StudyGotchi.Services.ServiceRegistry.PetController;
                var activePet = pc.GetActivePet();
                activePet?.SetName(name);
            }

            // Navigate to dashboard
            StudyGotchi.Services.ServiceRegistry.DashboardViewModel.Refresh();
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToDashboard();
        }

        public void OnSaveSettings()
        {
            // placeholder for saving settings
        }
    }
}