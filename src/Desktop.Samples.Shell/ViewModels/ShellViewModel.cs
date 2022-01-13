using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows;

namespace Desktop.Samples.Shell.ViewModels
{
    public class ShellViewModel : NotificationObject
    {
        public ShellViewModel()
        {

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
