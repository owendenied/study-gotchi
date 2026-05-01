using System.Windows.Controls;

namespace StudyGotchi.Views.UserControls
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            
            // Set initial state from backend
            if (StudyGotchi.Services.ServiceRegistry.SessionController != null)
            {
                SldDecayRate.Value = StudyGotchi.Services.ServiceRegistry.SessionController.HungerDecayRate;
            }
        }

        private void BtnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToPetSelection();
        }

        private void BtnLetsGo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var name = TxtPetName.Text?.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                var pc = StudyGotchi.Services.ServiceRegistry.PetController;
                var activePet = pc.GetActivePet();
                activePet?.SetName(name);
            }

            // Save decay rate
            StudyGotchi.Services.ServiceRegistry.SessionController.HungerDecayRate = (int)SldDecayRate.Value;

            // Navigate to Task Setup
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToTaskSetup();
        }

        public void OnSaveSettings()
        {
            // placeholder for saving settings
        }

        private void SldDecayRate_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (TxtDecayRateValue == null) return;
            
            int val = (int)e.NewValue;
            if (val == 1) TxtDecayRateValue.Text = "Currently: Low";
            else if (val == 2) TxtDecayRateValue.Text = "Currently: Medium";
            else if (val == 3) TxtDecayRateValue.Text = "Currently: High";
        }
    }
}