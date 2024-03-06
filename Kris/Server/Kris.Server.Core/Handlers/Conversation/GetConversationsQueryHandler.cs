using FluentResults;
using Kris.Interface.Models;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Conversation;

public sealed class GetConversationsQueryHandler : ConversationHandler, IRequestHandler<GetConversationsQuery, Result<IEnumerable<ConversationListModel>>>
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IConversationMapper _conversationMapper;

    public GetConversationsQueryHandler(IAuthorizationService authorizationService, IConversationMapper conversationMapper,
        IConversationRepository conversationRepository)
        : base(conversationRepository)
    {
        _authorizationService = authorizationService;
        _conversationMapper = conversationMapper;
    }

    public async Task<Result<IEnumerable<ConversationListModel>>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));
        var authResult = await _authorizationService.AuthorizeAsync(user, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var conversations = await _conversationRepository.GetByUserAsync(user.UserId, user.SessionId.Value, cancellationToken);

        return Result.Ok(conversations.Select(_conversationMapper.Map));
    }
}
