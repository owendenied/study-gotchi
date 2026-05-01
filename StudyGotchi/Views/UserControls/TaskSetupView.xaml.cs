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
            // Default hour to current hour + 1
            int defaultHour = Math.Min(DateTime.Now.Hour + 1, 23);
            CmbHour.SelectedIndex = defaultHour;
            CmbMinute.SelectedIndex = 0;
            UpdatePreview();

            // Update preview whenever user changes controls
            CmbHour.SelectionChanged += (s, e) => UpdatePreview();
            CmbMinute.SelectionChanged += (s, e) => UpdatePreview();
            ChkTomorrow.Checked += (s, e) => UpdatePreview();
            ChkTomorrow.Unchecked += (s, e) => UpdatePreview();
        }

        private void UpdatePreview()
        {
            var deadline = BuildDeadline();
            TxtDeadlinePreview.Text = $"Due: {deadline:ddd, MMM d} at {deadline:HH:mm}";
        }

        private DateTime BuildDeadline()
        {
            int hour = CmbHour.SelectedIndex >= 0 ? CmbHour.SelectedIndex : 23;
            // Minutes are in 5-min steps: index 0 = 00, index 1 = 05, ...
            int minute = CmbMinute.SelectedIndex >= 0 ? CmbMinute.SelectedIndex * 5 : 0;
            bool tomorrow = ChkTomorrow.IsChecked == true;

            DateTime baseDate = tomorrow ? DateTime.Today.AddDays(1) : DateTime.Today;
            return baseDate.AddHours(hour).AddMinutes(minute);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var wnd = Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToSettings();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            var wnd = Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToDashboard();
        }

        public void OnAddTask()
        {
            // placeholder
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            var name = TxtTaskName.Text?.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                var tc = StudyGotchi.Services.ServiceRegistry.TaskController;
                tc.AddTask(name, BuildDeadline());
            }

            // Clear inputs for the next task
            TxtTaskName.Clear();
            CmbHour.SelectedIndex = Math.Min(DateTime.Now.Hour + 1, 23);
            CmbMinute.SelectedIndex = 0;
            ChkTomorrow.IsChecked = false;

            var wnd = Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToDashboard();
        }
    }
}