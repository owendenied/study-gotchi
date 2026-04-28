using System.Windows.Controls;

namespace StudyGotchi.Views.UserControls
{
    public partial class SessionSummaryView : UserControl
    {
        public SessionSummaryView()
        {
            InitializeComponent();
            this.DataContext = StudyGotchi.Services.ServiceRegistry.DashboardViewModel;
        }

        private void BtnStartAnother_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StudyGotchi.Services.ServiceRegistry.ResetApp();
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToPetSelection();
        }

        private void BtnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void DisplaySummary()
        {
            // placeholder
        }
    }
}