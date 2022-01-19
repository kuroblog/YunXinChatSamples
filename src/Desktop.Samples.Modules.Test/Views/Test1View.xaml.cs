using Desktop.Samples.Common;
using Desktop.Samples.Modules.Test.ViewModels;
using Microsoft.Practices.Prism.Logging;
using System;
using System.Windows.Controls;

namespace Desktop.Samples.Modules.Test.Views
{
    public partial class Test1View : UserControl
    {
        private readonly ILoggerFacade _logger;

        public Test1View(
            Test1ViewModel viewModel,
            ILoggerFacade logger)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug($"{GetType().Name} ... ctor.");
        }
    }
}
