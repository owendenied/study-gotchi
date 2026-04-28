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
        }
    }
}