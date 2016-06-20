using Microsoft.Practices.Prism.PubSubEvents;

namespace PythogorasSquare.Clients.UWP.Wpf
{
    public static class EventAggregatorContext
    {
        public static IEventAggregator Current { get; internal set; }
    }
}