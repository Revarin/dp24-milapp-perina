using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Client.Core.Handlers.Message;

public sealed class SendMessageCommandHandler : MessageHandler, IRequestHandler<SendMessageCommand>
{
    private readonly IMessageHub _messageClient;

    public SendMessageCommandHandler(IMessageHub messageClient)
    {
        _messageClient = messageClient;
    }

    public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var httpRequest = new SendMessageRequest
        {
            ConversationId = request.ConversationId,
            Message = request.Body,
            Sent = DateTime.UtcNow
        };
        await _messageClient.SendMessage(httpRequest);
    }
}
