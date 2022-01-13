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
    public class HomeViewModel : NotificationObject, INavigationAware
    {
        private readonly ILoggerFacade _logger;
        private readonly IRegionManager _region;

        public DelegateCommand<UserControl> LoadedCommand
        {
            get => new DelegateCommand<UserControl>(OnLoaded);
        }

        public DelegateCommand GoToLoginCommand
        {
            get => new DelegateCommand(OnGoToLogin);
        }

        public HomeViewModel(
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

        private void OnGoToLogin()
        {
            _region.RequestNavigate(
                TestRegionNames.TestHome,
                new Uri(typeof(LoginView).FullName, UriKind.Relative),
                navigationResult => { });
        }

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var navigationDictArgsJson = string.Empty;
            if (navigationContext.NavigationService.Region.Context != null)
            {
                var dict = navigationContext.NavigationService.Region.Context.ToString().ParseTo<Dictionary<string, string>>();
                dict.TryGetValue(GetType().FullName, out navigationDictArgsJson);

                _logger.Debug($"{GetType().Name} ... {nameof(OnNavigatedTo)} ... {nameof(navigationDictArgsJson)}:{navigationDictArgsJson}.");
            }
        }
        #endregion
    }
}
