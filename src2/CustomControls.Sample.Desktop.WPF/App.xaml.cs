using System;
using System.Threading.Tasks;
using System.Windows;

namespace CustomControls.Sample.Desktop.WPF
{
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += (s, e) => { };
            TaskScheduler.UnobservedTaskException += (s, e) => { };
            AppDomain.CurrentDomain.UnhandledException += (s, e) => { };

            Exit += (s, e) => { };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnMainWindowClose;

            var bootstrapper = new ShellBootstrapper();
            bootstrapper.Run();
        }
    }
}
