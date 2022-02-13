using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CustomControls.Samples.WPF
{
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += OnThreadUnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += OnApplicationUnhandledException;

            Exit += OnExit;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) { }

        private void OnThreadUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e) { }

        private void OnApplicationUnhandledException(object sender, UnhandledExceptionEventArgs e) { }

        private void OnExit(object sender, ExitEventArgs e) { }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnMainWindowClose;

            var bootstrapper = new ShellBootstrapper();
            bootstrapper.Run();
        }
    }
}
