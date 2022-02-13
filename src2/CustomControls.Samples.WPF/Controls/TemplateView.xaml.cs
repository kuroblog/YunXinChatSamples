using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Windows.Controls;

namespace CustomControls.Samples.WPF.Controls
{
    public partial class TemplateView : UserControl
    {
        private readonly ILoggerFacade _logger;

        public TemplateView(
            TemplateViewModel viewModel,
            ILoggerFacade logger)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }
    }

    public class TemplateViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private string _title = "Template View";

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public TemplateViewModel(
            ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }
    }
}
