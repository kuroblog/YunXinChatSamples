using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace CustomControls.Sample.WPF
{
    public partial class TextBoxStylesPage : Page
    {
        public TextBoxStylesPage()
        {
            InitializeComponent();

            DataContext = new TextBoxStylesPageViewModel();
        }
    }

    public class ImageAutoSizeBehavior : Behavior<Image>
    {
        public static readonly DependencyProperty WidthLimitProperty = DependencyProperty.Register(
            nameof(WidthLimit),
            typeof(double),
            typeof(ImageAutoSizeBehavior),
            new PropertyMetadata(100d));

        public double WidthLimit
        {
            get
            {
                return (double)GetValue(WidthLimitProperty);
            }
            set
            {
                SetValue(WidthLimitProperty, value);
            }
        }

        private void OnLoadedHandle(object sender, RoutedEventArgs e)
        {
            var source = AssociatedObject.Source;
            if (source != null)
            {
                AssociatedObject.Width = source.Width > WidthLimit ? WidthLimit : source.Width;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += OnLoadedHandle;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= OnLoadedHandle;
            }
        }
    }

    public class TextBoxAcceptsReturnBehavior : Behavior<TextBox>
    {
        private void OnKeyDownHandle(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                var index = AssociatedObject.CaretIndex;
                AssociatedObject.Text = AssociatedObject.Text.Insert(index, Environment.NewLine);
                AssociatedObject.CaretIndex = index + 1;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.KeyDown += OnKeyDownHandle;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.KeyDown -= OnKeyDownHandle;
            }
        }
    }

    public class BringNewItemIntoViewBehavior : Behavior<ItemsControl>
    {
        private INotifyCollectionChanged notifier;

        protected override void OnAttached()
        {
            base.OnAttached();
            notifier = AssociatedObject.Items as INotifyCollectionChanged;
            notifier.CollectionChanged += ItemsControl_CollectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            notifier.CollectionChanged -= ItemsControl_CollectionChanged;
        }

        private void ItemsControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newIndex = e.NewStartingIndex;
                var newElement = AssociatedObject.ItemContainerGenerator.ContainerFromIndex(newIndex);
                var item = (FrameworkElement)newElement;
                item?.BringIntoView();
            }
        }
    }

    public enum MessageType
    {
        System = 0x00,
        Self = 0x01,
        From = 0x02
    }

    public class SessionContent : NotificationObject
    {
        private string _headerImage;
        private string _name;
        private string _message;
        private MessageType _messageType;
        private string _imageUri;
        private double _imageWidth;
        private double _imageHeight;

        public double ImageHeight
        {
            get
            {
                return _imageHeight;
            }
            set
            {
                _imageHeight = value;
                RaisePropertyChanged(() => ImageHeight);
            }
        }

        public double ImageWidth
        {
            get
            {
                return _imageWidth;
            }
            set
            {
                _imageWidth = value;
                RaisePropertyChanged(() => ImageWidth);
            }
        }

        public string ImageUri
        {
            get
            {
                return _imageUri;
            }
            set
            {
                _imageUri = value;
                RaisePropertyChanged(() => ImageUri);
            }
        }

        public MessageType MessageType
        {
            get
            {
                return _messageType;
            }
            set
            {
                _messageType = value;
                RaisePropertyChanged(() => MessageType);
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string HeaderImage
        {
            get
            {
                return _headerImage;
            }
            set
            {
                _headerImage = value;
                RaisePropertyChanged(() => HeaderImage);
            }
        }

        public SessionContent(string message = "", string imageUri = "", string name = "", string headerImageUri = "", MessageType type = MessageType.Self)
        {
            _message = message;
            _imageUri = imageUri;
            _name = name;
            _headerImage = headerImageUri;
            _messageType = type;
        }
    }

    public class TextBoxStylesPageViewModel : NotificationObject
    {
        private string _message;
        private ObservableCollection<SessionContent> _messages;

        public ObservableCollection<SessionContent> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                RaisePropertyChanged(() => Messages);
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        public DelegateCommand NewLLineCommand => new DelegateCommand(() =>
        {
            Message += Environment.NewLine;
        });

        public DelegateCommand MessageToCommand => new DelegateCommand(() =>
        {
            if (!string.IsNullOrEmpty(Message))
            {
                var imageUrl = "https://file-oss.ijia120.com/uploads/2019-10-06/5d999abeed95c.png";

                Messages.Add(
                    new SessionContent(
                        message: Message,
                        name: "测试一把",
                        headerImageUri: imageUrl,
                        type: MessageType.Self));

                Message = string.Empty;
            }
        });

        public DelegateCommand ImageToCommand => new DelegateCommand(() =>
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "open image",
                Filter = "image files(*.jpg,*.gif,*.bmp,*.png)|*.jpg;*.gif;*.bmp;*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var imageFile = openFileDialog.FileName;

                var imageUrl = "https://file-oss.ijia120.com/uploads/2019-10-06/5d999abeed95c.png";

                Messages.Add(
                    new SessionContent(
                        imageUri: imageFile,
                        name: "测试一把",
                        headerImageUri: imageUrl,
                        type: MessageType.Self));

            }
        });

        public DelegateCommand EmojiToCommand => new DelegateCommand(() => { });

        public DelegateCommand AudioToCommand => new DelegateCommand(() => { });

        public DelegateCommand VideoToCommand => new DelegateCommand(() => { });

        public DelegateCommand MockFromMessageCommand => new DelegateCommand(() =>
        {
            var imageUrl = "https://file-oss.ijia120.com/uploads/2019-10-06/5d999a84645c3.png";
            Messages.Add(
                new SessionContent(
                    message: $"随便发了一把测试的消息, {Guid.NewGuid().ToString("N")}",
                    name: "测试两把",
                    headerImageUri: imageUrl,
                    type: MessageType.From));
        });

        public DelegateCommand MockSystemMessageCommand => new DelegateCommand(() =>
        {
            Messages.Add(
                new SessionContent(
                    message: "这是一条测试用的系统消息, 不要在意 ...",
                    type: MessageType.System));
        });

        public DelegateCommand MockMessagesCleanCommand => new DelegateCommand(() => Messages.Clear());

        public TextBoxStylesPageViewModel()
        {
            _messages = new ObservableCollection<SessionContent>();
        }
    }
}
