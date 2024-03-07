using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class SendMessageCommand : IRequest
{
    public Guid ConversationId { get; set; }
    public string Body { get; set; }
}
