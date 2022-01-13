using Desktop.Samples.Common;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Windows;

namespace Desktop.Samples.Shell.ViewModels
{
    public class ShellViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public DelegateCommand<Window> LoadedCommand
        {
            get => new DelegateCommand<Window>(OnLoaded);
        }

        public ShellViewModel(
            ILoggerFacade logger)
        {
            _title = "Desktop Samples";

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger?.Debug($"{GetType().FullName} ... ctor.");
        }

        private void OnLoaded(Window window)
        {
            _logger.Debug($"{GetType().Name} ... {nameof(OnLoaded)} ... {nameof(window)}:{window?.GetType().Name}.");
        }
    }
}
