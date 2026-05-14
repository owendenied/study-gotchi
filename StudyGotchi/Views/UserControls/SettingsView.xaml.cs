using System.Windows.Controls;
using StudyGotchi.Controllers;

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

            TxtPetName.Text = StudyGotchi.Services.ServiceRegistry.PetController.GetPetName();

            var audioSvc = StudyGotchi.Services.ServiceRegistry.AudioService;
            if (audioSvc != null)
            {
                BtnMuteToggle.IsChecked = audioSvc.IsMuted;
            }

            BtnReminderToggle.IsChecked = StudyGotchi.Services.ServiceRegistry.ReminderService.IsEnabled;

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
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            var name = TxtPetName.Text?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                wnd?.ShowToast("Give your pet a name first.");
                return;
            }

            if (name.Length > PetController.MaxPetNameLength)
            {
                wnd?.ShowToast($"Keep pet names under {PetController.MaxPetNameLength} characters.");
                return;
            }

            if (!string.IsNullOrEmpty(name))
            {
                var pc = StudyGotchi.Services.ServiceRegistry.PetController;
                var activePet = pc.GetActivePet();
                activePet?.SetName(PetController.NormalizePetName(name));
            }

            SaveSettings();

            // Navigate to Task Setup
            wnd?.NavigateToTaskSetup();
        }

        public void OnSaveSettings()
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            StudyGotchi.Services.ServiceRegistry.SessionController.HungerDecayRate = (int)SldDecayRate.Value;
            StudyGotchi.Services.ServiceRegistry.SaveState();
        }

        private void BtnMuteToggle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Primitives.ToggleButton;
            if (btn == null) return;

            var audioSvc = StudyGotchi.Services.ServiceRegistry.AudioService;
            if (audioSvc != null)
            {
                audioSvc.IsMuted = btn.IsChecked == true;
                SaveSettings();
            }
        }

        private void BtnReminderToggle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Primitives.ToggleButton;
            if (btn == null) return;

            StudyGotchi.Services.ServiceRegistry.ReminderService.IsEnabled = btn.IsChecked == true;
            SaveSettings();
        }

        private void BtnFullScreen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.ToggleFullScreen();
        }

        private void BtnResetSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show(
                "Start fresh? This clears the local pet, tasks, settings, and session stats saved on this computer.",
                "Start Fresh",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (result != System.Windows.MessageBoxResult.Yes) return;

            StudyGotchi.Services.ServiceRegistry.ResetSaveData();
            TxtPetName.Text = StudyGotchi.Services.ServiceRegistry.PetController.GetPetName();
            SldDecayRate.Value = StudyGotchi.Services.ServiceRegistry.SessionController.HungerDecayRate;
            BtnMuteToggle.IsChecked = StudyGotchi.Services.ServiceRegistry.AudioService.IsMuted;
            BtnReminderToggle.IsChecked = StudyGotchi.Services.ServiceRegistry.ReminderService.IsEnabled;

            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToPetSelection();
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
