using FluentResults;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class RemoveEmptyConversationsCommand : IRequest<Result>
{
    public required Guid SessionId { get; set; }
}
