using FluentResults;
using Kris.Interface.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class GetMessagesQuery : AuthentizedRequest, IRequest<Result<IEnumerable<MessageModel>>>
{
    public required Guid ConversationId { get; set; }
    public int? Count { get; set; }
    public int? From { get; set; }
}
