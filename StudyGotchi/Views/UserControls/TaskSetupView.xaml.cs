using System.Windows.Controls;

namespace StudyGotchi.Views.UserControls
{
    public partial class TaskSetupView : UserControl
    {
        public TaskSetupView()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToPetSelection();
        }

        public void OnAddTask()
        {
            // placeholder
        }

        private void BtnAddTask_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // For now, after adding a task navigate to dashboard
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToDashboard();
        }
    }
}