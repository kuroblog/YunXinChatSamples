using System;
using System.Windows;
using CustomControls.Samples.WPF.Controls;
using CustomControls.Samples.WPF.Utils;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
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

            void doRegisterControl<TControl>(string name = "", LifetimeManager lifetime = null)
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

            var region = Container.Resolve<IRegionViewRegistry>() ?? throw new ArgumentNullException($"{nameof(IRegionViewRegistry)} resolve failed.");

            region.RegisterViewWithRegion(RegionNames.Root, () => Container.Resolve<MainView>());
            region.RegisterViewWithRegion(RegionNames.Main, () => Container.Resolve<HomeView>());

            doRegisterControl<TemplateView>();
            doRegisterControl<Test1View>();
            doRegisterControl<Test2View>();
            doRegisterControl<ImageView>();
            doRegisterControl<InputView>();
            doRegisterControl<BackgroundWorkerView>();
            doRegisterControl<CefBrowserView>();
        }

        protected override ILoggerFacade CreateLogger()
        {
            //return base.CreateLogger();

            return new NLogWrapper();
        }
    }
}
