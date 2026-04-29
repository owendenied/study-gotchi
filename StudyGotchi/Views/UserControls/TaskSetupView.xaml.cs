using System;
using System.Windows;
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
            wnd?.NavigateToSettings();
        }

        private void BtnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToDashboard();
        }

        public void OnAddTask()
        {
            // placeholder
        }

        private void BtnAddTask_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var name = TxtTaskName.Text?.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                var tc = StudyGotchi.Services.ServiceRegistry.TaskController;
                tc.AddTask(name, DpDeadline.SelectedDate ?? DateTime.Now.AddDays(1));
            }

            var wnd = System.Windows.Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToDashboard();
        }
    }
}