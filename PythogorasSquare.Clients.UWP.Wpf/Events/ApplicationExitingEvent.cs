using Microsoft.Practices.Prism.PubSubEvents;
using PythogorasSquare.Common.Threading;

namespace PythogorasSquare.Clients.UWP.Wpf.Events
{
    public class ApplicationExitingEvent : PubSubEvent<IDeferredOperation>
    {

    }
}