using CustomControls.Sample.Desktop.WPF.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CustomControls.Sample.Desktop.WPF
{
    public partial class ShellWindow : Window
    {
        public ShellWindow(
            ShellWindowModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }

    public class ShellWindowModel : NotificationObject
    {
        private string _title = "WPF Samples";
        private ObservableCollection<MenuItemInfo> _menus = new ObservableCollection<MenuItemInfo>();
        private Frame _contentFrame;

        private readonly IRegionManager _region;

        public ObservableCollection<MenuItemInfo> Menus
        {
            get { return _menus; }
            set
            {
                _menus = value;
                RaisePropertyChanged(() => Menus);
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public DelegateCommand<Frame> FrameLoadedCommand { get; set; }

        public DelegateCommand<object> ContentCloseCommand { get; set; }

        public ShellWindowModel(
            IRegionManager region)
        {
            _region = region ?? throw new ArgumentNullException(nameof(region));

            FrameLoadedCommand = new DelegateCommand<Frame>(OnFrameLoaded);
            ContentCloseCommand = new DelegateCommand<object>(OnContentClose);

            void doNavigationHandle(string uriPath)
            {
                //_contentFrame.Source = new Uri(uriPath, UriKind.Relative);
                _region.RequestNavigate("Content", new Uri(uriPath, UriKind.Relative), result => { });
            }

            void undoNavigationHandle(string arg) { }

            MenuItemInfo addMenuItemInfo(ObservableCollection<MenuItemInfo> menus, string title, string key, Action<string> handle)
            {
                var menu = new MenuItemInfo(title, key, handle);
                menus.Add(menu);

                return menu;
            }

            var m1 = addMenuItemInfo(_menus, "Styles", "", undoNavigationHandle);
            addMenuItemInfo(m1.SubMenus, "Custom List", typeof(CustomListView).FullName, doNavigationHandle);
            addMenuItemInfo(m1.SubMenus, "User List MVVM", typeof(UserListMvvmView).FullName, doNavigationHandle);
            addMenuItemInfo(m1.SubMenus, "User List", typeof(UserListView).FullName, doNavigationHandle);
            addMenuItemInfo(m1.SubMenus, "Test-1", typeof(Test1View).FullName, doNavigationHandle);
            addMenuItemInfo(m1.SubMenus, "Test-2", typeof(Test2View).FullName, doNavigationHandle);

            var m2 = addMenuItemInfo(_menus, "Menu 2", "menu2", undoNavigationHandle);
            var m3 = addMenuItemInfo(_menus, "Menu 3", "menu3", undoNavigationHandle);
        }

        private void OnFrameLoaded(Frame frame)
        {
            _contentFrame = frame;
        }

        private void OnContentClose(object item)
        {
            if (item is TabItem tabItem)
            {
                var view = tabItem.DataContext;
                tabItem.Template = null;

                if (view != null)
                {
                    _region.Regions["Content"].Remove(view);
                }
                else
                {
                    //TODO: tabitem's datacontext is null.
                }
            }
        }
    }

    public class MenuItemInfo
    {
        public string Title { get; set; }

        public string NavigationKey { get; set; }

        public DelegateCommand<string> NavigationCommand { get; set; }

        public ObservableCollection<MenuItemInfo> SubMenus { get; set; }

        public MenuItemInfo(string title, string key, Action<string> handle)
        {
            Title = title;
            NavigationKey = key;
            NavigationCommand = new DelegateCommand<string>(handle);

            SubMenus = new ObservableCollection<MenuItemInfo>();
        }
    }
}
