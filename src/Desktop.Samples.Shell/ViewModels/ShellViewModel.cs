using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows;

namespace Desktop.Samples.Shell.ViewModels
{
    public class ShellViewModel : NotificationObject
    {
        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public ShellViewModel()
        {
            _title = "Desktop Samples";
        }

        private void OnLoaded(Window window)
        {

        }

        public DelegateCommand<Window> LoadedCommand
        {
            get => new DelegateCommand<Window>(OnLoaded);
        }
    }
}
