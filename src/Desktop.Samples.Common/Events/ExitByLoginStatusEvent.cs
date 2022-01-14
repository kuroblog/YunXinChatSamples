using Microsoft.Practices.Prism.Events;
using System;

namespace Desktop.Samples.Common.Events
{
    public class ExitByLoginStatusEvent : CompositePresentationEvent<bool> { }

    public static class ExitByLoginStatusEventExtensions
    {
        public static void PublishExitByLoginStatusEvent(this IEventAggregator @event, bool loginStatus)
        {
            @event?.GetEvent<ExitByLoginStatusEvent>().Publish(loginStatus);
        }

        public static void SubscribeExitByLoginStatusEvent(this IEventAggregator @event, Action<bool> handler)
        {
            @event?.GetEvent<ExitByLoginStatusEvent>().Subscribe(handler);
        }
    }
}
