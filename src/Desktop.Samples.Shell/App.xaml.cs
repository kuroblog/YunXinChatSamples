using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Desktop.Samples.Shell
{
    public partial class App : Application
    {
        public App()
        {
            Exit += OnExit;
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += OnTasksUnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
        }

        private void OnTasksUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
        }

        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
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
