using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace CustomControls.Samples.WPF.Controls
{
    public partial class InputView : UserControl
    {
        private readonly ILoggerFacade _logger;

        public InputView(
            InputViewModel viewModel,
            ILoggerFacade logger)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }
    }

    public class InputViewModel : NotificationObject, IDataErrorInfo
    {
        private readonly ILoggerFacade _logger;
        private string _title = "Input View";

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public InputViewModel(
            ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }

        private string _input;

        [Required(ErrorMessage = "not empty")]
        [StringLength(6, ErrorMessage = "lenght more than 6")]
        public string Input
        {
            get
            {
                return _input;
            }
            set
            {
                _input = value;
                RaisePropertyChanged(() => Input);
            }
        }

        private string _phone;

        [Required(ErrorMessage = "not empty")]
        [RegularExpression("^1[34578]\\d{9}$", ErrorMessage = "invalid mobile phone number")]
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                RaisePropertyChanged(() => Phone);
            }
        }

        public string Error { get; private set; }

        public string this[string columnName]
        {
            get
            {
                var vc = new ValidationContext(this, null, null);
                vc.MemberName = columnName;
                var res = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                var result = Validator.TryValidateProperty(GetType().GetProperty(columnName).GetValue(this, null), vc, res);
                if (res.Count > 0)
                {
                    return string.Join(Environment.NewLine, res.Select(a => a.ErrorMessage)?.ToArray());
                }
                return string.Empty;

                //var propertyInfo = GetType().GetProperty(columnName);
                //var propertyValue = propertyInfo.GetValue(this, null);

                //var validationContext = new ValidationContext(this, null, null) { MemberName = columnName };

                //var errorResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                //var isValid = Validator.TryValidateProperty(propertyValue, validationContext, errorResults);

                //if (isValid)
                //{
                //    Error = string.Empty;
                //    return Error;
                //}

                //Error = string.Join(Environment.NewLine, errorResults.Select(a => a.ErrorMessage).ToArray());

                //return errorResults.FirstOrDefault().ErrorMessage;
            }
        }
    }

    public class ErrorContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<ValidationError> errors)
            {
                return errors.FirstOrDefault()?.ErrorContent;
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
