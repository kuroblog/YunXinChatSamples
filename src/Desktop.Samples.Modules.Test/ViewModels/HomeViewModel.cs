using Desktop.Samples.Common;
using Desktop.Samples.Common.Events;
using Desktop.Samples.Common.YunXinSDKs;
using Desktop.Samples.Modules.Test.Views;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
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
        private readonly IEventAggregator _event;
        private YunXinService _yunxin;

        public DelegateCommand<UserControl> LoadedCommand
        {
            get => new DelegateCommand<UserControl>(OnLoaded);
        }

        public DelegateCommand GoToLoginCommand
        {
            get => new DelegateCommand(OnGoToLogin);
        }

        public DelegateCommand LogoutCommand
        {
            get => new DelegateCommand(OnLogout);
        }

        public DelegateCommand NavigationToFriendViewCommand
        {
            get => new DelegateCommand(() => OnNavigationToView(typeof(FriendView).FullName));
        }

        public DelegateCommand NavigationToSessionViewCommand
        {
            get => new DelegateCommand(() => OnNavigationToView(typeof(SessionView).FullName));
        }

        public HomeViewModel(
            YunXinService yunxin,
            IEventAggregator @event,
            IRegionManager region,
            ILoggerFacade logger)
        {
            _yunxin = yunxin ?? throw new ArgumentNullException(nameof(yunxin));
            _yunxin.RegisterCallbacks();

            _event = @event ?? throw new ArgumentNullException(nameof(@event));
            _region = region ?? throw new ArgumentNullException(nameof(region));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger?.Debug($"{GetType().FullName} ... ctor.");
        }

        private void OnLoaded(UserControl control)
        {
            _event.PublishExitByLoginStatusEvent(true);

            _yunxin.RegisterEventCallbacks();

            _logger.Debug($"{GetType().Name} ... {nameof(OnLoaded)} ... {nameof(control)}:{control?.GetType().Name}.");
        }

        private void OnGoToLogin()
        {
            _region.RequestNavigate(
                TestRegionNames.TestHome,
                new Uri(typeof(LoginView).FullName, UriKind.Relative),
                navigationResult => { });
        }

        private void OnLogout()
        {
            _yunxin.Logout(logoutResult =>
            {
                _event.PublishExitByLoginStatusEvent(false);

                _yunxin.ReleaseEventCallbacks();

                MainDispatcher.Instance.Invoke(() =>
                {
                    _region.RequestNavigate(
                        TestRegionNames.TestHome,
                        new Uri(typeof(LoginView).FullName, UriKind.Relative),
                        navigationResult => { });
                });
            });
        }

        private void OnNavigationToView(string viewName)
        {
            MainDispatcher.Instance.Invoke(() =>
            {
                _region.RequestNavigate(
                    TestRegionNames.TestContent,
                    new Uri(viewName, UriKind.Relative),
                    navigationResult => { });
            });
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
