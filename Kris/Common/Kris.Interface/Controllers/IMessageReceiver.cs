using Kris.Interface.Models;

namespace Kris.Interface.Controllers;

public interface IMessageReceiver
{
    Task ReceiveMessage(MessageModel message, CancellationToken ct);
}
