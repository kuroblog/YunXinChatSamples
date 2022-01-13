using Desktop.Samples.Common;
using Desktop.Samples.Shell.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.Practices.Prism.Logging;
using System;

namespace Desktop.Samples.Shell.Views
{
    public partial class ShellView : MetroWindow //System.Windows.Window
    {
        private readonly ILoggerFacade _logger;

        public ShellView(
            ShellViewModel viewModel,
            ILoggerFacade logger)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug($"{GetType().Name} ... ctor.");
        }
    }
}
