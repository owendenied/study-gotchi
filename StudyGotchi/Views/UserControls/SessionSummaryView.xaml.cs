using System.Windows.Controls;

namespace StudyGotchi.Views.UserControls
{
    public partial class SessionSummaryView : UserControl
    {
        public SessionSummaryView()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToDashboard();
        }

        public void DisplaySummary()
        {
            // placeholder
        }
    }
}