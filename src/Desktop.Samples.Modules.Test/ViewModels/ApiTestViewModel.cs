using Desktop.Samples.Common;
using Desktop.Samples.Common.YunXinSDKs;
using Desktop.Samples.Modules.Test.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Windows.Controls;

namespace Desktop.Samples.Modules.Test.ViewModels
{
    public class ApiTestViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private YunXinService _yunxin;
        private ApiService _api;

        public DelegateCommand<UserControl> LoadedCommand
        {
            get => new DelegateCommand<UserControl>(OnLoaded);
        }

        public DelegateCommand ApiTestCommand
        {
            get => new DelegateCommand(OnApiTest);
        }

        public ApiTestViewModel(
            ApiService api,
            YunXinService yunxin,
            ILoggerFacade logger)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _yunxin = yunxin ?? throw new ArgumentNullException(nameof(yunxin));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger?.Debug($"{GetType().FullName} ... ctor.");
        }

        private void OnLoaded(UserControl control)
        {
            _yunxin.GetFriends(friend =>
            {
                _logger.Debug($"... {nameof(friend)}:{friend.ToJson(true)}");
            });

            _logger.Debug($"{GetType().Name} ... {nameof(OnLoaded)} ... {nameof(control)}:{control?.GetType().Name}.");
        }

        public void OnApiTest()
        {
            var result1 = _api.SendCode();

            var result2 = _api.LoginWithSmsCode();

            var result3 = _api.GetToken(result2.data.loginCode);
        }
    }
}
