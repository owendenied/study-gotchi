// App.xaml.cs

using System.Windows;

namespace StudyGotchi
{
    // Starts the app. App.xaml points it straight to MainSetupWindow.
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            System.AppDomain.CurrentDomain.UnhandledException += (s, ex) =>
            {
                System.IO.File.WriteAllText("crash_log.txt", ex.ExceptionObject.ToString());
            };

            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.Primitives.ButtonBase), 
                System.Windows.Controls.Primitives.ButtonBase.ClickEvent, 
                new RoutedEventHandler(GlobalClickHandler));
        }

        private void GlobalClickHandler(object sender, RoutedEventArgs e)
        {
            // Don't play click for CheckBox since it triggers the task complete sound
            if (e.OriginalSource is System.Windows.Controls.CheckBox) return;
            
            StudyGotchi.Services.ServiceRegistry.AudioService?.PlaySfx("sfx_click");
        }
    }
}