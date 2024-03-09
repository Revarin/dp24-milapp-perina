using Kris.Client.Common.Events;
using Kris.Client.Connection.Hubs.Events;

namespace Kris.Client.Connection.Hubs;

public interface IMessageReceiver : Interface.Controllers.IMessageReceiver
{
    event EventHandler<MessageReceivedEventArgs> MessageReceived;
    bool IsConnected { get; }
    Task Connect();
    Task Disconnect();
}
