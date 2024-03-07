using Kris.Interface.Models;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface IMessageReceiver
{
    Task ReceiveMessage(MessageModel message);
    Task ReceiveError(Response response);
}
