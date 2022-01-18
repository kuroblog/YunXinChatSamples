using Desktop.Samples.Common;
using Desktop.Samples.Common.Events;
using Desktop.Samples.Common.YunXinSDKs;
using Desktop.Samples.Modules.Test.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace Desktop.Samples.Modules.Test.ViewModels
{
    public class FriendViewModel : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private readonly IEventAggregator _event;
        private YunXinService _yunxin;
        private ObservableCollection<UserProfile> _friends = new ObservableCollection<UserProfile>();

        private UserProfile _selectedFriend;

        public UserProfile SelectedFriend
        {
            get => _selectedFriend;
            set
            {
                _selectedFriend = value;
                RaisePropertyChanged(() => SelectedFriend);
            }
        }

        public ObservableCollection<UserProfile> Friends
        {
            get => _friends;
            set
            {
                _friends = value;
                RaisePropertyChanged(() => Friends);
            }
        }

        public DelegateCommand<UserControl> LoadedCommand
        {
            get => new DelegateCommand<UserControl>(OnLoaded);
        }

        public FriendViewModel(
            YunXinService yunxin,
            IEventAggregator @event,
            ILoggerFacade logger)
        {
            _yunxin = yunxin ?? throw new ArgumentNullException(nameof(yunxin));

            _event = @event ?? throw new ArgumentNullException(nameof(@event));
            _event.SubscribeYunXinUserOnlineEvent(SetUserOnloine);

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger?.Debug($"{GetType().FullName} ... ctor.");
        }

        private void SetUserOnloine(YunXinUserOnlineEventArgs arg)
        {
            var friend = Friends.FirstOrDefault(u => u.Id == arg.UserId);
            if (friend != null)
            {
                friend.IsOnline = arg.IsOnline;
            }
        }

        private void BindingFriends(List<UserProfile> friends)
        {
            Friends.Clear();

            friends?.ForEach(f => Friends.Add(f));

            SelectedFriend = Friends?.FirstOrDefault();
        }

        private void OnLoaded(UserControl control)
        {
            BindingFriends(null);

            _yunxin.GetFriends(friend =>
            {
                MainDispatcher.Instance.Invoke(() =>
                {
                    var friends = friend?.ProfileList?.Select(f => new UserProfile { Id = f.AccountId, Alias = f.Alias })?.ToList();
                    BindingFriends(friends);
                });

                _logger.Debug($"... {nameof(friend)}:{friend.ToJson(true)}");
            });

            _logger.Debug($"{GetType().Name} ... {nameof(OnLoaded)} ... {nameof(control)}:{control?.GetType().Name}.");
        }
    }
}
