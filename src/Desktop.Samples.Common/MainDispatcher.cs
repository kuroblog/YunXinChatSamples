using System;
using System.Windows;
using System.Windows.Threading;

namespace Desktop.Samples.Common
{
    public sealed class MainDispatcher
    {
        private static readonly Lazy<MainDispatcher> _mainDispatcher = new Lazy<MainDispatcher>(() => new MainDispatcher());

        public static MainDispatcher Instance => _mainDispatcher.Value;

        private MainDispatcher() { }

        //private readonly PrismApplication app = Application.Current as PrismApplication;
        private readonly Application app = Application.Current;

        public void Invoke(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            app.Dispatcher.BeginInvoke(priority, action);
        }

        //public IContainerProvider Container => app.Container;

        //public void ShowMessage(string message, string title = "温馨提示", Action<INotification> action = null)
        //{
        //    Container?.Resolve<IEventAggregator>()?.GetEvent<MainNotificationPopupEvent>()?.Publish(new PopupEventArg<INotification>
        //    {
        //        Title = title,
        //        Content = message,
        //        Callback = action
        //    });
        //}
    }
}
