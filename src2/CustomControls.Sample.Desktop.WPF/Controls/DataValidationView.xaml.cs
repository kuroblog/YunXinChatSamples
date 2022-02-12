using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace CustomControls.Sample.Desktop.WPF.Controls
{
    public partial class DataValidationView : UserControl
    {
        public DataValidationView(DataValidationViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }

    public class DataValidationViewModel : NotificationObject, IDataErrorInfo
    {
        private string _title = "Data Validation";
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

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
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

        public DelegateCommand TestCommand => new DelegateCommand(() =>
        {
            Debug.WriteLine($" ... error:{Error}.");
        });

        public DataValidationViewModel()
        {

        }
    }
}
