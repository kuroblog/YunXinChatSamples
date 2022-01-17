using Desktop.Samples.Common;
using Desktop.Samples.Common.YunXinSDKs;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Windows.Controls;

namespace Desktop.Samples.Modules.Test.ViewModels
{
    public class FriendViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private YunXinService _yunxin;

        public DelegateCommand<UserControl> LoadedCommand
        {
            get => new DelegateCommand<UserControl>(OnLoaded);
        }

        public FriendViewModel(
            YunXinService yunxin,
            ILoggerFacade logger)
        {
            _yunxin = yunxin ?? throw new ArgumentNullException(nameof(yunxin));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger?.Debug($"{GetType().FullName} ... ctor.");
        }

        private void OnLoaded(UserControl control)
        {
            _yunxin.GetFriends(friend =>
            {
                _logger.Debug($"... friend:{friend.ToJson(true)}");
            });

            _logger.Debug($"{GetType().Name} ... {nameof(OnLoaded)} ... {nameof(control)}:{control?.GetType().Name}.");
        }
    }
}
