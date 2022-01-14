﻿using Desktop.Samples.Common;
using Desktop.Samples.Common.Events;
using Desktop.Samples.Common.YunXinSDKs;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Desktop.Samples.Shell
{
    public partial class App : Application
    {
        private ILoggerFacade _logger;
        private YunXinService _yunxin;
        private bool _loginStatus;

        public App()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += OnTasksUnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
            Exit += OnExit;
        }

        private void OnErrorHandler(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                var errorLog = ex.ToErrorText();
                MessageBox.Show(errorLog, "error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            OnErrorHandler(() =>
            {
                _logger?.Error(e.Exception);
                e.Handled = true;
                // or
                //Application.Current.Shutdown(-1);
            });
        }

        private void OnTasksUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            OnErrorHandler(() =>
            {
                _logger.Error(e.Exception);
                e.SetObserved();
            });
        }

        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            OnErrorHandler(() => _logger.Error(e.ExceptionObject as Exception));
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            _yunxin?.Clean(_loginStatus);
            //_yunxin?.Clean();

            _logger.Debug($"{GetType().Name} ... {nameof(OnExit)}.");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnMainWindowClose;

            var bootstrapper = new ShellBootstrapper();
            bootstrapper.Run();

            var @event = bootstrapper.Container.Resolve<IEventAggregator>();
            if (@event == null)
            {
                throw new ArgumentException($"{nameof(IEventAggregator)} resolve failed.");
            }
            @event.SubscribeExitByLoginStatusEvent(status => _loginStatus = status);

            _yunxin = bootstrapper.Container.Resolve<YunXinService>();
            if (_yunxin == null)
            {
                throw new ArgumentException($"{nameof(YunXinService)} resolve failed.");
            }
            var isYunXinSDKInitialized = _yunxin.Init("YunXinSDKSamples");
            if (isYunXinSDKInitialized == false)
            {
                MessageBox.Show("YunXin SDKs initialization failed.", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }

            _logger = bootstrapper.Container.Resolve<ILoggerFacade>();
            if (_logger == null)
            {
                throw new ArgumentException($"{nameof(ILoggerFacade)} resolve failed.");
            }
            _logger.Debug($"{GetType().Name} ... {nameof(OnStartup)}.");
        }
    }
}
