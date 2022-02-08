using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace CustomControls.Sample.Desktop.WPF.Controls
{
    public partial class UserListView : UserControl
    {
        public UserListView(UserListViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }

    public class UserListViewModel : NotificationObject
    {
        private string _title = "User List";
        private ObservableCollection<UserGroupInfo> _userGroups;
        private ObservableCollection<UserInfo> _users;
        private ListBox _userListBox;

        public ObservableCollection<UserInfo> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        public ObservableCollection<UserGroupInfo> UserGroups
        {
            get
            {
                return _userGroups;
            }
            set
            {
                _userGroups = value;
                RaisePropertyChanged(() => UserGroups);
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

        public DelegateCommand<ListBox> ListBoxLoadedCommand { get; set; }

        public UserListViewModel()
        {
            ListBoxLoadedCommand = new DelegateCommand<ListBox>(OnListBoxLoaded);

            _userGroups = new ObservableCollection<UserGroupInfo>();

            var imageUrl = "https://file-oss.ijia120.com/uploads/2019-10-06/5d999abeed95c.png";

            var users = new List<UserInfo> {
                new UserInfo("张三", "ZhangSan", imageUrl, new []{ "哮喘", "家族遗传", "帕金森病移苗", "咳嗽" }),
                new UserInfo("梅钰", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("张洺", "ZhangMing", imageUrl, null),
                new UserInfo("王玥", "WangYue", imageUrl, null),
                new UserInfo("王思琪", "WangSiQi", imageUrl, null),
                new UserInfo("董云强", "DongYunQiang", imageUrl, null),
                new UserInfo("宋红培", "SongHongPei", imageUrl, null),
                new UserInfo("石磊", "ShiLei", imageUrl, null),
                new UserInfo("亚历山大", "", imageUrl, null)}.OrderBy(a => a.GroupKey);

            _users = new ObservableCollection<UserInfo>(users);

            var userGroups = _users
                .GroupBy(a => a.PinYin.ToCharArray().FirstOrDefault())
                .Select(a => new UserGroupInfo(a.Key.ToString(), new ObservableCollection<UserInfo>(a)))?
                .ToArray();

            _userGroups = new ObservableCollection<UserGroupInfo>(userGroups);

        }

        private void OnListBoxLoaded(ListBox listBox)
        {
            // TODO: 会反复添加 key
            _userListBox = listBox;

            var cv = CollectionViewSource.GetDefaultView(_userListBox.ItemsSource);
            cv.GroupDescriptions.Add(new PropertyGroupDescription(nameof(UserInfo.GroupKey)));
        }
    }

    public class UserGroupInfo
    {
        public string NameKey { get; set; }

        public ObservableCollection<UserInfo> Users { get; set; }

        public UserGroupInfo(string nameKey, ObservableCollection<UserInfo> users)
        {
            NameKey = nameKey;
            Users = users;
        }
    }

    public class UserInfo
    {
        public string GroupKey
        {
            get => string.IsNullOrEmpty(PinYin) ? "" : PinYin.ToCharArray().FirstOrDefault().ToString();
        }

        public string Name { get; set; }

        public string PinYin { get; set; }

        public string HeaderImage { get; set; }

        public ObservableCollection<string> Tags { get; set; }

        public UserInfo(string name, string pinyin, string headerImage, string[] tags = null)
        {
            Name = name;
            PinYin = pinyin;
            HeaderImage = headerImage;

            if (tags == null)
            {
                Tags = new ObservableCollection<string>();
            }
            else
            {
                Tags = new ObservableCollection<string>(tags);
            }
        }
    }
}
