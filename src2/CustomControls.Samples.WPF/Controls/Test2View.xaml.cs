using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Windows.Controls;

namespace CustomControls.Samples.WPF.Controls
{
    public partial class Test2View : UserControl
    {
        private readonly ILoggerFacade _logger;

        public Test2View(
            Test2ViewModel viewModel,
            ILoggerFacade logger)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }
    }

    public class Test2ViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private string _title = "Test2 View";

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public Test2ViewModel(
            ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }
    }
}
