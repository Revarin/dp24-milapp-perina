using FluentResults;
using Kris.Interface.Requests;
using Kris.Server.Core.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class SendMessageCommand : AuthentizedRequest, IRequest<Result<MessageNotificationModel>>
{
    public required SendMessageRequest SendMessage { get; set; }
}
