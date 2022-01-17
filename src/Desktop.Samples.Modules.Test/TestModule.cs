using Desktop.Samples.Common;
using Desktop.Samples.Modules.Test.Views;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;

namespace Desktop.Samples.Modules.Test
{
    public class TestModule : IModule
    {
        private readonly ILoggerFacade _logger;
        private readonly IRegionManager _region;
        private readonly IUnityContainer _container;

        public TestModule(
            IUnityContainer container,
            IRegionManager region,
            ILoggerFacade logger)
        {
            _region = region ?? throw new ArgumentNullException(nameof(region));
            _container = container ?? throw new ArgumentNullException(nameof(container));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger?.Debug($"{GetType().Name} ... ctor.");
        }

        public void Initialize()
        {
            _region.RegisterViewWithRegion(RegionNames.Main, () => _container.Resolve<MainView>());
            _region.RegisterViewWithRegion(TestRegionNames.TestHome, () => _container.Resolve<LoginView>());

            _container.RegisterType<object, HomeView>(typeof(HomeView).FullName, new PerResolveLifetimeManager());

            _container.RegisterType<object, FriendView>(typeof(FriendView).FullName, new PerResolveLifetimeManager());
            _container.RegisterType<object, SessionView>(typeof(SessionView).FullName, new PerResolveLifetimeManager());

            _logger?.Debug($"{GetType().Name} ... {nameof(Initialize)}.");
        }
    }
}
