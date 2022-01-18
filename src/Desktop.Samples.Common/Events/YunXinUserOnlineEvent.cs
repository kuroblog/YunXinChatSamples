using Microsoft.Practices.Prism.Events;
using System;

namespace Desktop.Samples.Common.Events
{
    public class YunXinUserOnlineEventArgs
    {
        public string UserId { get; set; }

        public bool IsOnline { get; set; }
    }

    public class YunXinUserOnlineEvent : CompositePresentationEvent<YunXinUserOnlineEventArgs> { }

    public static class YunXinUserOnlineEventExtensions
    {
        public static void PublishYunXinUserOnlineEvent(this IEventAggregator @event, YunXinUserOnlineEventArgs args)
        {
            @event?.GetEvent<YunXinUserOnlineEvent>().Publish(args);
        }

        public static void SubscribeYunXinUserOnlineEvent(this IEventAggregator @event, Action<YunXinUserOnlineEventArgs> handler)
        {
            @event?.GetEvent<YunXinUserOnlineEvent>().Subscribe(handler);
        }
    }
}
