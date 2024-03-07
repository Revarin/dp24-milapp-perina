using Kris.Common.Extensions;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRSwaggerGen.Attributes;

namespace Kris.Server.Controllers;

[SignalRHub]
public class MessageHub : KrisHub<IMessageReceiver>, IMessageHub
{
    public MessageHub(IMediator mediator) : base(mediator)
    {
    }

    [Authorize]
    public async Task SendMessage(SendMessageRequest request)
    {
        var user = CurrentUser();
        if (user == null)
        {
            await Clients.Caller.ReceiveError(Unauthorized());
            return;
        }

        var ct = new CancellationToken();
        var command = new SendMessageCommand { User = user, SendMessage = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) await Clients.Caller.ReceiveError(Unauthorized(result.Errors.FirstMessage()));
            else if (result.HasError<EntityNotFoundError>()) await Clients.Caller.ReceiveError(NotFound(result.Errors.FirstMessage()));
            else await Clients.Caller.ReceiveError(InternalError());

            return;
        }

        var notification = result.Value;
        await Clients.Users(notification.UsersToNotify.Select(id => id.ToString()).ToList())
            .ReceiveMessage(notification.Message);

        return;
    }
}
