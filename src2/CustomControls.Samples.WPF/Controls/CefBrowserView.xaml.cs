using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Windows.Controls;

namespace CustomControls.Samples.WPF.Controls
{
    public partial class CefBrowserView : UserControl
    {
        private readonly ILoggerFacade _logger;

        public CefBrowserView(
            CefBrowserViewModel viewModel,
            ILoggerFacade logger)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }
    }

    public class CefBrowserViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private string _title = "Cef Browser View";

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public CefBrowserViewModel(
            ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }

        private string _address = "https://www.baidu.com";

        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                RaisePropertyChanged(() => Address);
            }
        }

        private string _requestUri;

        public string RequestUri
        {
            get
            {
                return _requestUri;
            }
            set
            {
                _requestUri = value;
                RaisePropertyChanged(() => RequestUri);
            }
        }

        public DelegateCommand GoCommand => new DelegateCommand(() => RequestUri = Address);
    }
}
