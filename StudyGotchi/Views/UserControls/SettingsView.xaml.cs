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

            var audioSvc = StudyGotchi.Services.ServiceRegistry.AudioService;
            if (audioSvc != null)
            {
                BtnMuteToggle.IsChecked = audioSvc.IsMuted;
            }

            this.Loaded += (s, e) =>
            {
                var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
                if (wnd != null)
                {
                    BtnFullScreen.IsChecked = wnd.WindowStyle == System.Windows.WindowStyle.None;
                }
            };
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

        private void BtnMuteToggle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Primitives.ToggleButton;
            if (btn == null) return;

            var audioSvc = StudyGotchi.Services.ServiceRegistry.AudioService;
            if (audioSvc != null)
            {
                audioSvc.IsMuted = btn.IsChecked == true;
            }
        }

        private void BtnFullScreen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.ToggleFullScreen();
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