using System.Windows;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace CustomControls.Samples.WPF
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

            void doRegisterControl<TControl>(string name, LifetimeManager lifetime = null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = typeof(TControl).FullName;
                }

                if (lifetime == null)
                {
                    lifetime = new PerResolveLifetimeManager();
                }

                Container.RegisterType<object, TControl>(name, lifetime);
            }

            // register controls
        }

        protected override ILoggerFacade CreateLogger()
        {
            //return base.CreateLogger();

            return new NLogWrapper();
        }
    }
}
