using FluentResults;
using Kris.Common.Enums;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Models;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Conversation;

public sealed class SendMessageCommandHandler : ConversationHandler, IRequestHandler<SendMessageCommand, Result<MessageNotificationModel>>
{
    private readonly IMessageMapper _messageMapper;

    public SendMessageCommandHandler(IMessageMapper messageMapper,
        IConversationRepository conversationRepository, IAuthorizationService authorizationService)
        : base(conversationRepository, authorizationService)
    {
        _messageMapper = messageMapper;
    }

    public async Task<Result<MessageNotificationModel>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        var authResult = await _authorizationService.AuthorizeAsync(user, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var conversation = await _conversationRepository.GetWithAllAsync(request.SendMessage.ConversationId, cancellationToken);
        if (conversation == null) return Result.Fail(new EntityNotFoundError("Conversation", request.SendMessage.ConversationId));
        if (!conversation.Users.Any(sessionUser => sessionUser.Id == authResult.SessionUserId))
            return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var newMessage = new MessageEntity
        {
            ConversationId = conversation.Id,
            MessageType = MessageType.Generic,
            SenderId = authResult.SessionUserId,
            TimeStamp = request.SendMessage.Sent,
            Body = request.SendMessage.Message
        };
        conversation.Messages.Add(newMessage);
        await _conversationRepository.UpdateAsync(cancellationToken);

        return Result.Ok(new MessageNotificationModel
        {
            Message = _messageMapper.Map(newMessage),
            UsersToNotify = conversation.Users.Select(sessionUser => sessionUser.UserId).ToList()
        });
    }
}
