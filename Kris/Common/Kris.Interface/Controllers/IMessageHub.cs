using Kris.Interface.Requests;
using Kris.Interface.Responses;

namespace Kris.Interface.Controllers;

public interface IMessageHub
{
    Task<Response?> SendMessage(SendMessageRequest request);
}
