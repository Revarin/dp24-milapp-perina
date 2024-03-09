using FluentResults;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class DeleteConversationCommand : IRequest<Result>
{
    public Guid ConversationId { get; set; }
}
