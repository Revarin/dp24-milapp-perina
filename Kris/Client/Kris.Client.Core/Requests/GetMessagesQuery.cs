using FluentResults;
using Kris.Client.Core.Models;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class GetMessagesQuery : IRequest<Result<IEnumerable<MessageModel>>>
{
    public Guid ConversationId { get; set; }
    public int Page { get; set; }
}
