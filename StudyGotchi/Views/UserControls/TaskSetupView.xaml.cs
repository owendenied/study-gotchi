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
            
            CmbMinute.Items.Clear();
            for (int i = 0; i < 60; i++)
            {
                CmbMinute.Items.Add(new ComboBoxItem { Content = i.ToString("D2") });
            }

            SetDefaultTime();
            UpdatePreview();

            // Update preview whenever user changes controls
            CmbHour.SelectionChanged += (s, e) => UpdatePreview();
            CmbMinute.SelectionChanged += (s, e) => UpdatePreview();
            CmbAmPm.SelectionChanged += (s, e) => UpdatePreview();
            ChkTomorrow.Checked += (s, e) => UpdatePreview();
            ChkTomorrow.Unchecked += (s, e) => UpdatePreview();
        }

        private void SetDefaultTime()
        {
            DateTime dt = DateTime.Now.AddHours(1);
            CmbAmPm.SelectedIndex = dt.Hour >= 12 ? 1 : 0;
            int displayHour = dt.Hour % 12;
            if (displayHour == 0) displayHour = 12;
            CmbHour.SelectedIndex = displayHour - 1;
            CmbMinute.SelectedIndex = 0;
            ChkTomorrow.IsChecked = false;
        }

        private void UpdatePreview()
        {
            var deadline = BuildDeadline();
            TxtDeadlinePreview.Text = $"Due: {deadline:ddd, MMM d} at {deadline:hh:mm tt}";
        }

        private DateTime BuildDeadline()
        {
            int hour12 = CmbHour.SelectedIndex >= 0 ? CmbHour.SelectedIndex + 1 : 12;
            bool isPm = CmbAmPm.SelectedIndex == 1;

            int hour24;
            if (isPm)
            {
                hour24 = hour12 == 12 ? 12 : hour12 + 12;
            }
            else
            {
                hour24 = hour12 == 12 ? 0 : hour12;
            }

            // Minutes are in 1-min steps: index 0 = 00, index 1 = 01, ...
            int minute = CmbMinute.SelectedIndex >= 0 ? CmbMinute.SelectedIndex : 0;
            bool tomorrow = ChkTomorrow.IsChecked == true;

            DateTime baseDate = tomorrow ? DateTime.Today.AddDays(1) : DateTime.Today;
            return baseDate.AddHours(hour24).AddMinutes(minute);
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
            SetDefaultTime();

            var wnd = Window.GetWindow(this) as Views.MainWindow;
            wnd?.NavigateToDashboard();
        }
    }
}