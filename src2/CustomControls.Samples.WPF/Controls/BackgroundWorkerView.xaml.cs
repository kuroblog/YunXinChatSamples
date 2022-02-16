using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace CustomControls.Samples.WPF.Controls
{
    public partial class BackgroundWorkerView : UserControl
    {
        private readonly ILoggerFacade _logger;

        public BackgroundWorkerView(
            BackgroundWorkerViewModel viewModel,
            ILoggerFacade logger)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }
    }

    public class BackgroundWorkerViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private string _title = "Background Worker View";

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public BackgroundWorkerViewModel(
            ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");

            _bgWorker.WorkerReportsProgress = true;
            _bgWorker.DoWork += OnDoWork;
            _bgWorker.ProgressChanged += OnProgressChanged;
            _bgWorker.RunWorkerCompleted += OnRunWorkerCompleted;
        }

        public DelegateCommand<object> LoadedCommand => new DelegateCommand<object>(obj =>
        {
            _bgWorker.RunWorkerAsync();
        });

        private BackgroundWorker _bgWorker = new BackgroundWorker();

        private void OnDoWork(object sender, DoWorkEventArgs e)
        {
            if (sender is BackgroundWorker worker)
            {
                worker.ReportProgress(99);
            }
        }

        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var processValue = e.ProgressPercentage;
        }

        private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
    }
}
