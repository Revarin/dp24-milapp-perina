using Kris.Common.Extensions;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SignalRSwaggerGen.Attributes;

namespace Kris.Server.Controllers;

[SignalRHub]
public class MessageHub : KrisHub<IMessageReceiver>, IMessageHub
{
    public MessageHub(IMediator mediator) : base(mediator)
    {
    }

    [Authorize]
    public async Task<Response?> SendMessage(SendMessageRequest request)
    {
        var user = CurrentUser();
        if (user == null) return Unauthorized();

        var ct = new CancellationToken();
        var command = new SendMessageCommand { User = user, SendMessage = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Unauthorized(result.Errors.FirstMessage());
            else if (result.HasError<EntityNotFoundError>()) return NotFound(result.Errors.FirstMessage());
            else return InternalError();
        }

        var notification = result.Value;
        await Clients.Users(notification.UsersToNotify.Select(id => id.ToString()).ToList())
            .ReceiveMessage(notification.Message);

        return Ok();
    }
}
