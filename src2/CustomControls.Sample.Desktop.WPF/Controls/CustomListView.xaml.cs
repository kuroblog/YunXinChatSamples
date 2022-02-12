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
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomControls.Sample.Desktop.WPF.Controls
{
    public partial class CustomListView : UserControl
    {
        public CustomListView(CustomListViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }

    public class CustomListViewModel : NotificationObject
    {
        private string _title = "Custom List";

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

        private ObservableCollection<PatientInfo> _patients = new ObservableCollection<PatientInfo>();

        public ObservableCollection<PatientInfo> Patients
        {
            get
            {
                return _patients;
            }
            set
            {
                _patients = value;
                RaisePropertyChanged(() => Patients);
            }
        }

        public CustomListViewModel()
        {
            void doNavigation(PatientInfo patient)
            {

            }

            Patients.Clear();

            Patients.Add(new PatientInfo(0, "待接诊", "张三", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(0, "待接诊", "张三1", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(0, "待接诊", "张三2", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(1, "问诊中", "张三6", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(1, "问诊中", "张三7", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(0, "待接诊", "张三3", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(2, "已完成", "张三a", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(0, "待接诊", "张三4", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(3, "已关闭", "张三b", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(0, "待接诊", "张三5", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(1, "问诊中", "张三8", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(2, "已完成", "张三9", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
            Patients.Add(new PatientInfo(3, "已关闭", "张三c", "女", 99, "我的患者", "2020-12-12 12:00", "ddddddddddddddddd", doNavigation));
        }
    }

    public class PatientInfo
    {
        public int State { get; set; }

        public string StateDesc { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }

        public string BizType { get; set; }

        public string Date { get; set; }

        public string OrderNo { get; set; }

        public DelegateCommand<PatientInfo> NavigationCommand { get; set; }

        public PatientInfo(int state, string stateDesc, string name, string sex, int age, string bizType, string date, string orderNo, Action<PatientInfo> navigationHandle = null)
        {
            State = state;
            StateDesc = stateDesc;
            Name = name;
            Sex = sex;
            Age = age;
            BizType = bizType;
            Date = date;
            OrderNo = orderNo;

            if (navigationHandle != null)
            {
                NavigationCommand = new DelegateCommand<PatientInfo>(navigationHandle);
            }
        }
    }

    //public class ListViewColumnAutoSizeBehavior : Behavior<ListView>
    //{
    //    protected override void OnAttached()
    //    {
    //        base.OnAttached();

    //        AssociatedObject.Loaded += OnLoaded;
    //    }

    //    protected override void OnDetaching()
    //    {
    //        base.OnDetaching();

    //        if (AssociatedObject != null)
    //        {
    //            AssociatedObject.Loaded -= OnLoaded;
    //        }
    //    }

    //    private void OnLoaded(object sender, RoutedEventArgs e)
    //    {
    //        if (AssociatedObject.View is GridView gv)
    //        {
    //            foreach (var item in gv.Columns)
    //            {
    //                gv.AllowsColumnReorder
    //            }
    //        }
    //    }
    //}
}
