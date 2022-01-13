using Desktop.Samples.Shell.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using System;
using System.Windows;

namespace Desktop.Samples.Shell
{
    public class ShellBootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            if (Shell is Window window)
            {
                Application.Current.MainWindow = window;
                Application.Current.MainWindow.Show();
            }
            else
            {
                throw new ArgumentException("Shell type is not a System.Windows.Window");
            }
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            //return base.CreateModuleCatalog();

            return new DirectoryModuleCatalog { ModulePath = @".\" };
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        }
    }
}
