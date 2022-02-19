using CustomControls.Samples.WPF.Utils;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace CustomControls.Samples.WPF.Controls
{
    public partial class HomeView : UserControl
    {
        private readonly ILoggerFacade _logger;

        public HomeView(
            HomeViewModel viewModel,
            ILoggerFacade logger)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");
        }
    }

    public class HomeViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private readonly IRegionManager _region;
        private ObservableCollection<MenuItemInfo> _menus = new ObservableCollection<MenuItemInfo>();

        public ObservableCollection<MenuItemInfo> Menus
        {
            get { return _menus; }
            set
            {
                _menus = value;
                RaisePropertyChanged(() => Menus);
            }
        }

        public DelegateCommand<object> ItemCloseCommand
        {
            get => new DelegateCommand<object>(item =>
            {
                if (item is TabItem tabItem)
                {
                    var view = tabItem.DataContext;
                    tabItem.Template = null;

                    if (view != null)
                    {
                        _region.Regions[RegionNames.Content].Remove(view);
                    }
                    else
                    {
                        //TODO: tabitem's datacontext is null.
                    }
                }
            });
        }

        public HomeViewModel(
            IRegionManager region,
            ILoggerFacade logger)
        {
            _region = region ?? throw new ArgumentNullException(nameof(region));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Debug("ctor.");

            void doNavigationHandler(string uriPath)
            {
                //_contentFrame.Source = new Uri(uriPath, UriKind.Relative);
                _region.RequestNavigate(RegionNames.Content, new Uri(uriPath, UriKind.Relative), result => { });
            }

            void undoNavigationHandler(string arg) { }

            MenuItemInfo addMenuItemInfo(ObservableCollection<MenuItemInfo> menus, string title, string key, Action<string> handle)
            {
                var menu = new MenuItemInfo(title, key, handle);
                menus.Add(menu);

                return menu;
            }

            var controlMenu = addMenuItemInfo(_menus, "Contorls", "", undoNavigationHandler);
            addMenuItemInfo(controlMenu.SubMenus, "Image", typeof(ImageView).FullName, doNavigationHandler);
            addMenuItemInfo(controlMenu.SubMenus, "Input", typeof(InputView).FullName, doNavigationHandler);
            addMenuItemInfo(controlMenu.SubMenus, "BackgroundWorker", typeof(BackgroundWorkerView).FullName, doNavigationHandler);
            addMenuItemInfo(controlMenu.SubMenus, "CefWebBrowser", typeof(CefBrowserView).FullName, doNavigationHandler);

            var testMenu = addMenuItemInfo(_menus, "Tests", "", undoNavigationHandler);
            addMenuItemInfo(testMenu.SubMenus, "Template", typeof(TemplateView).FullName, doNavigationHandler);
            addMenuItemInfo(testMenu.SubMenus, "Test1", typeof(Test1View).FullName, doNavigationHandler);
            addMenuItemInfo(testMenu.SubMenus, "Test2", typeof(Test2View).FullName, doNavigationHandler);
        }
    }

    public class MenuItemInfo
    {
        public string Title { get; set; }

        public string NavigationKey { get; set; }

        public DelegateCommand<string> NavigationCommand { get; set; }

        public ObservableCollection<MenuItemInfo> SubMenus { get; set; }

        public MenuItemInfo(string title, string key, Action<string> navigationHandler)
        {
            Title = title;
            NavigationKey = key;

            if (navigationHandler != null)
            {
                NavigationCommand = new DelegateCommand<string>(navigationHandler);
            }

            SubMenus = new ObservableCollection<MenuItemInfo>();
        }
    }
}
