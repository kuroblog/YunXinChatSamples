using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Windows.Controls;

namespace CustomControls.Samples.WPF.Controls
{
    public partial class ImageView : UserControl
    {
        private readonly ILoggerFacade _logger;

        public ImageView(
            ImageViewModel viewModel,
            ILoggerFacade logger)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }
    }

    public class ImageViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private string _title = "Image View";

        private string _imageUri = "https://cdn-mp-oss.ijia120.com/ih-patient/2021-12-14/ih-patient/1639461235027-2051342.jpg";

        public string ImageUri
        {
            get => _imageUri;
            set
            {
                _imageUri = value;
                RaisePropertyChanged(() => ImageUri);
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public ImageViewModel(
            ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }
    }
}
