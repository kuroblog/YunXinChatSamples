using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomControls.Sample.Desktop.WPF.Controls
{
    public partial class UserListMvvmView : UserControl
    {
        public UserListMvvmView(UserListMvvmViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }

    public class UserListMvvmViewModel : NotificationObject
    {
        private string _title = "User List MVVM";
        private ObservableCollection<UserInfo> _users;

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

        public UserListMvvmViewModel()
        {
            var imageUrl = "https://file-oss.ijia120.com/uploads/2019-10-06/5d999abeed95c.png";

            var users = new List<UserInfo> {
                new UserInfo("张三", "ZhangSan", imageUrl, new []{ "哮喘", "家族遗传", "帕金森病移苗", "咳嗽" }),
                new UserInfo("梅钰", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰0", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰1", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰2", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰3", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰4", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰5", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰6", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰7", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰8", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰9", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰a", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰b", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰c", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰d", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰e", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰f", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰g", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("梅钰h", "MeiYu", imageUrl, new []{ "哮喘", "家族遗传", "风湿" }),
                new UserInfo("张洺", "ZhangMing", imageUrl, null),
                new UserInfo("王玥", "WangYue", imageUrl, null),
                new UserInfo("王思琪", "WangSiQi", imageUrl, null),
                new UserInfo("董云强", "DongYunQiang", imageUrl, null),
                new UserInfo("宋红培", "SongHongPei", imageUrl, null),
                new UserInfo("石磊", "ShiLei", imageUrl, null),
                new UserInfo("亚历山大", "", imageUrl, null)}.OrderBy(a => a.GroupKey);

            _users = new ObservableCollection<UserInfo>(users);
        }
    }
}
