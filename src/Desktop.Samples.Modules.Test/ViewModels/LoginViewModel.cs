using Desktop.Samples.Common;
using Desktop.Samples.Modules.Test.Views;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Desktop.Samples.Modules.Test.ViewModels
{
    public class LoginViewModel : NotificationObject, INavigationAware
    {
        private readonly ILoggerFacade _logger;
        private readonly IRegionManager _region;

        public DelegateCommand<UserControl> LoadedCommand
        {
            get => new DelegateCommand<UserControl>(OnLoaded);
        }

        public DelegateCommand GoToHomeCommand
        {
            get => new DelegateCommand(OnGoToHome);
        }

        public LoginViewModel(
            IRegionManager region,
            ILoggerFacade logger)
        {
            _region = region ?? throw new ArgumentNullException(nameof(region));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger?.Debug($"{GetType().FullName} ... ctor.");
        }

        private void OnLoaded(UserControl control)
        {
            _logger.Debug($"{GetType().Name} ... {nameof(OnLoaded)} ... {nameof(control)}:{control?.GetType().Name}.");
        }

        private void OnGoToHome()
        {
            _region.RequestNavigate(
                TestRegionNames.TestHome,
                new Uri(typeof(HomeView).FullName, UriKind.Relative),
                navigationResult => { });
        }

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            var navigationDictArgs = new Dictionary<string, string> { { typeof(HomeViewModel).FullName, "go to home" } }.ToJson();
            navigationContext.NavigationService.Region.Context = navigationDictArgs;

            _logger.Debug($"{GetType().Name} ... {nameof(OnNavigatedFrom)} ... {nameof(navigationDictArgs)}:{navigationDictArgs}.");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
        #endregion
    }
}
