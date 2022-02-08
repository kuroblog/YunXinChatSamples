using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Windows.Controls;

namespace CustomControls.Sample.Desktop.WPF.Controls
{
    public partial class Test1View : UserControl
    {
        public Test1View(Test1ViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }

    public class Test1ViewModel : NotificationObject
    {
        private string _title = "Test 1";

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

        public Test1ViewModel()
        {

        }
    }
}
