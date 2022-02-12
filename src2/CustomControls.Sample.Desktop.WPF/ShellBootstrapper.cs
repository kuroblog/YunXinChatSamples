using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using Microsoft.Practices.Unity;
using CustomControls.Sample.Desktop.WPF.Controls;

namespace CustomControls.Sample.Desktop.WPF
{
    public class ShellBootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = Shell as Window;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            void registerView<T>(LifetimeManager lifetimeManager = null)
            {
                if (lifetimeManager == null)
                {
                    lifetimeManager = new PerResolveLifetimeManager();
                }

                Container.RegisterType<object, T>(typeof(T).FullName, lifetimeManager);
            }

            registerView<Test1View>();
            registerView<Test2View>();

            registerView<UserListView>();
            registerView<UserListMvvmView>();
            registerView<CustomListView>();
        }
    }
}
