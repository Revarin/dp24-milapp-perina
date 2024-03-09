using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Client.Core.Handlers.Message;

public sealed class SendMessageCommandHandler : MessageHandler, IRequestHandler<SendMessageCommand, Result>
{
    private readonly IMessageHub _messageClient;

    public SendMessageCommandHandler(IMessageHub messageClient)
    {
        _messageClient = messageClient;
    }

    public async Task<Result> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var httpRequest = new SendMessageRequest
        {
            ConversationId = request.ConversationId,
            Message = request.Body,
            Sent = DateTime.UtcNow
        };
        var response = await _messageClient.SendMessage(httpRequest);
        if (response == null) return Result.Fail(new ServerError("No response"));

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok();
    }
}
