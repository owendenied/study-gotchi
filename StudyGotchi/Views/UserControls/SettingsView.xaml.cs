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
            wnd?.NavigateToPetSelection();
        }

        public void OnSaveSettings()
        {
            // placeholder for saving settings
        }
    }
}