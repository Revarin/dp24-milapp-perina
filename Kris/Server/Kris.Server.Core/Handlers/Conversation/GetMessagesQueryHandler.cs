using FluentResults;
using Kris.Interface.Models;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Conversation;

public sealed class GetMessagesQueryHandler : ConversationHandler, IRequestHandler<GetMessagesQuery, Result<IEnumerable<MessageModel>>>
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IMessageRepository _messageRepository;
    private readonly IMessageMapper _messageMapper;

    public GetMessagesQueryHandler(IAuthorizationService authorizationService, IMessageRepository messageRepository,
        IMessageMapper messageMapper, IConversationRepository conversationRepository)
        : base(conversationRepository)
    {
        _authorizationService = authorizationService;
        _messageRepository = messageRepository;
        _messageMapper = messageMapper;
    }

    public async Task<Result<IEnumerable<MessageModel>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var user = request.User;
        var authResult = await _authorizationService.AuthorizeAsync(user, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var conversation = await _conversationRepository.GetWithUsersAsync(request.ConversationId, cancellationToken);
        if (conversation == null) return Result.Fail(new EntityNotFoundError("Conversation", request.ConversationId));
        if (!conversation.Users.Any(sessionUser => sessionUser.Id == authResult.SessionUserId))
            return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        IEnumerable<MessageEntity> messages;
        if (request.Count.HasValue && request.From.HasValue)
            messages = await _messageRepository.GetByConversationAsync(request.ConversationId, request.Count.Value, request.From.Value, cancellationToken);
        else if (request.Count.HasValue)
            messages = await _messageRepository.GetByConversationAsync(request.ConversationId, request.Count.Value, cancellationToken);
        else
            messages = await _messageRepository.GetByConversationAsync(request.ConversationId, cancellationToken);

        return Result.Ok(messages.Select(_messageMapper.Map));
    }
}
