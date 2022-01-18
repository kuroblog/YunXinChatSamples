using Microsoft.Practices.Prism.ViewModel;

namespace Desktop.Samples.Modules.Test.Models
{
    public class UserProfile : NotificationObject
    {
        private string _id;
        private string _secret;
        private string _alias;
        private bool _isOnline;
        private string _nickName;

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged(() => Id);
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

        public string Alias
        {
            get => _alias;
            set
            {
                _alias = value;
                RaisePropertyChanged(() => Alias);
            }
        }

        public bool IsOnline
        {
            get => _isOnline;
            set
            {
                _isOnline = value;
                RaisePropertyChanged(() => IsOnline);
            }
        }

        public string NickName
        {
            get => _nickName;
            set
            {
                _nickName = value;
                RaisePropertyChanged(() => NickName);
            }
        }
    }
}
