using Kris.Interface.Requests;

namespace Kris.Interface.Controllers;

public interface IMessageHub
{
    Task SendMessage(SendMessageRequest request, CancellationToken ct);
}
