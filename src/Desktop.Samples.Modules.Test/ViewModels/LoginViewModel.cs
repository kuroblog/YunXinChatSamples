using Desktop.Samples.Common;
using Desktop.Samples.Common.YunXinSDKs;
using Desktop.Samples.Modules.Test.Views;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace Desktop.Samples.Modules.Test.ViewModels
{
    public class LoginViewModel : NotificationObject, INavigationAware
    {
        private readonly ILoggerFacade _logger;
        private readonly IRegionManager _region;
        private LoginInfo _login;
        private ProxyInfo _proxy;
        private YunXinService _yunxin;

        public ProxyInfo Proxy
        {
            get => _proxy;
            set
            {
                _proxy = value;
                RaisePropertyChanged(() => Proxy);
            }
        }

        public LoginInfo Login
        {
            get => _login;
            set
            {
                _login = value;
                RaisePropertyChanged(() => Login);
            }
        }

        public DelegateCommand<UserControl> LoadedCommand
        {
            get => new DelegateCommand<UserControl>(OnLoaded);
        }

        public DelegateCommand GoToHomeCommand
        {
            get => new DelegateCommand(OnGoToHome);
        }

        //public DelegateCommand LoginCommand
        //{
        //    get => new DelegateCommand(OnLogin, CanLogin);
        //}

        public LoginViewModel(
            YunXinService yunxin,
            IRegionManager region,
            ILoggerFacade logger)
        {
            _login = new LoginInfo
            {
                LoginCommand = new DelegateCommand(OnLogin, CanLogin)
            };

            _proxy = new ProxyInfo
            {
                SetProxyCommand = new DelegateCommand(OnSetProxy)
            };

            _yunxin = yunxin ?? throw new ArgumentNullException(nameof(yunxin));
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

        private void OnLogin()
        {

        }

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(Login.LoginId)
                && !string.IsNullOrEmpty(Login.LoginPass)
                && Login.LoginId.Length > 2
                && Login.LoginPass.Length > 2;
        }

        private void OnSetProxy()
        {
            if (_proxy.EnableProxy)
            {
                _yunxin.SetProxy(_proxy.Type.Type, _proxy.IP, _proxy.Port, _proxy.User, _proxy.Secret);
            }
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

    public class LoginInfo : NotificationObject
    {
        private string _loginId;

        public string LoginId
        {
            get => _loginId;
            set
            {
                _loginId = value;
                RaisePropertyChanged(() => LoginId);

                LoginCommand?.RaiseCanExecuteChanged();
            }
        }

        private string _loginPass;

        public string LoginPass
        {
            get => _loginPass;
            set
            {
                _loginPass = value;
                RaisePropertyChanged(() => LoginPass);

                LoginCommand?.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand LoginCommand { get; set; }
    }

    public class ProxyInfo : NotificationObject
    {
        private bool _enableProxy = false;
        private string _ip = "127.0.0.1";
        private int _port = 8081;
        private string _user = string.Empty;
        private string _secret = string.Empty;
        private ObservableCollection<ProxyType> _proxyTypes = new ObservableCollection<ProxyType>
        {
            new ProxyType{ Desc = "Socks4", Type = NIM.NIMProxyType.kNIMProxySocks4 },
            new ProxyType{ Desc = "Socks4a", Type = NIM.NIMProxyType.kNIMProxySocks4a },
            new ProxyType{ Desc = "Socks5", Type = NIM.NIMProxyType.kNIMProxySocks5 }
        };

        public bool EnableProxy
        {
            get => _enableProxy;
            set
            {
                _enableProxy = value;
                RaisePropertyChanged(() => EnableProxy);

                if (value)
                {
                    SetProxyCommand?.Execute();
                }
            }
        }

        public string IP
        {
            get => _ip;
            set
            {
                _ip = value;
                RaisePropertyChanged(() => IP);
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                RaisePropertyChanged(() => Port);
            }
        }

        public string User
        {
            get => _user;
            set
            {
                _user = value;
                RaisePropertyChanged(() => User);
            }
        }

        public string Secret
        {
            get => _secret;
            set
            {
                _secret = value;
                RaisePropertyChanged(() => Secret);
            }
        }

        private ProxyType _type;

        public ProxyType Type
        {
            get => _type;
            set
            {
                _type = value;
                RaisePropertyChanged(() => Type);
            }
        }

        public ObservableCollection<ProxyType> ProxyTypes
        {
            get => _proxyTypes;
            set
            {
                _proxyTypes = value;
                RaisePropertyChanged(() => ProxyTypes);
            }
        }

        public DelegateCommand SetProxyCommand { get; set; }

        public ProxyInfo()
        {
            _type = _proxyTypes.FirstOrDefault();
        }
    }

    public class ProxyType
    {
        public string Desc { get; set; }

        public NIM.NIMProxyType Type { get; set; }
    }
}
