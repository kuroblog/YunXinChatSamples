using Microsoft.Practices.Prism.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Unity;

namespace CustomControls.Samples.WPF
{
    public partial class App : Application
    {
        private ILoggerFacade _logger;

        public App()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += OnThreadUnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += OnApplicationUnhandledException;

            Exit += OnExit;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ExceptionHandler(() =>
            {
                _logger.Error(e.Exception);
                e.Handled = true;
                // or
                //Application.Current.Shutdown(-1);
            });
        }

        private void OnThreadUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            ExceptionHandler(() =>
            {
                _logger.Error(e.Exception);
                e.SetObserved();
            });
        }

        private void OnApplicationUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ExceptionHandler(() =>
            {
                if (e.ExceptionObject is Exception error)
                {
                    _logger.Error(error);
                }
                else
                {
                    // TODO: throw unhandled error
                }
            });
        }

        private void ExceptionHandler(Action errorHandler)
        {
            try
            {
                errorHandler.Invoke();
            }
            catch (Exception ex)
            {
                var errorText = ex.ToErrorText();

                MessageBox.Show(errorText, "error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            _logger.Info($"exit {Environment.NewLine}");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnMainWindowClose;

            var bootstrapper = new ShellBootstrapper();
            bootstrapper.Run();

            _logger = bootstrapper.Container.Resolve<ILoggerFacade>() ?? throw new ArgumentNullException($"{nameof(ILoggerFacade)} resolve failed.");
            _logger.Debug(nameof(OnStartup));
        }
    }
}
