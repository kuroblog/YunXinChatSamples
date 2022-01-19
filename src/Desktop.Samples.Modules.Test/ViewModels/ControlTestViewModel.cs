using Desktop.Samples.Common;
using Desktop.Samples.Modules.Test.Views;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Windows.Controls;

namespace Desktop.Samples.Modules.Test.ViewModels
{
    public class ControlTestViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private readonly IRegionManager _region;

        public DelegateCommand<UserControl> LoadedCommand
        {
            get => new DelegateCommand<UserControl>(OnLoaded);
        }

        public DelegateCommand NavigationToTest1ViewCommand
        {
            get => new DelegateCommand(() => OnNavigationToView(typeof(Test1View).FullName));
        }

        public DelegateCommand NavigationToTest2ViewCommand
        {
            get => new DelegateCommand(() => OnNavigationToView(typeof(Test2View).FullName));
        }

        public DelegateCommand NavigationToTest3ViewCommand
        {
            get => new DelegateCommand(() => OnNavigationToView(typeof(Test3View).FullName));
        }

        public DelegateCommand<object> ViewCloseCommand
        {
            get => new DelegateCommand<object>(view =>
            {
                _region.Regions[TestRegionNames.TestTabContent].Remove(view);
            });
        }

        public ControlTestViewModel(
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

        private void OnNavigationToView(string viewName)
        {
            MainDispatcher.Instance.Invoke(() =>
            {
                _region.RequestNavigate(
                    TestRegionNames.TestTabContent,
                    new Uri(viewName, UriKind.Relative),
                    navigationResult => { });
            });
        }
    }
}
