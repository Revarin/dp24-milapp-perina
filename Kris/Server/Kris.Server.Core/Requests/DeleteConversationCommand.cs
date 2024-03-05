using FluentResults;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class DeleteConversationCommand : AuthentizedRequest, IRequest<Result>
{
    public required Guid ConversationId { get; set; }
}
