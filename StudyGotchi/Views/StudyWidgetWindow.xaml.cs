using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StudyGotchi.Controllers;

namespace StudyGotchi.Views
{
    public partial class StudyWidgetWindow : Window
    {

        public StudyWidgetWindow()
        {
            InitializeComponent();
            this.DataContext = StudyGotchi.Services.ServiceRegistry.DashboardViewModel;
        }

        public void UpdatePetDisplay()
        {
            // placeholder to refresh visual state from controllers
        }

        public void ReturnToDashboard()
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                // double-click detected: show main window (dashboard) and close widget
                var main = Application.Current?.Windows.OfType<MainWindow>().FirstOrDefault();
                if (main != null)
                {
                    main.Show();
                    main.NavigateToDashboard();
                }
                this.Close();
            }
            else
            {
                // start drag move on single click
                try { this.DragMove(); } catch { }
            }
        }
    }
}