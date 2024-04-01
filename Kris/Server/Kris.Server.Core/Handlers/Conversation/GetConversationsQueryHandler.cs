using FluentResults;
using Kris.Common.Enums;
using Kris.Interface.Models;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Conversation;

public sealed class GetConversationsQueryHandler : ConversationHandler, IRequestHandler<GetConversationsQuery, Result<IEnumerable<ConversationListModel>>>
{
    public GetConversationsQueryHandler(IConversationRepository conversationRepository, IAuthorizationService authorizationService)
        : base(conversationRepository, authorizationService)
    {
    }

    public async Task<Result<IEnumerable<ConversationListModel>>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));
        var authResult = await _authorizationService.AuthorizeAsync(user, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var conversations = await _conversationRepository.GetByUserAsync(user.UserId, user.SessionId.Value, cancellationToken);

        return Result.Ok(conversations.Select(conversation => new ConversationListModel
        {
            Id = conversation.Id,
            ConversationType = conversation.ConversationType,
            Name = conversation.ConversationType switch
            {
                ConversationType.Global => "Global Chat",
                ConversationType.Group => "Group Chat",
                ConversationType.Direct => conversation.Users.FirstOrDefault(u => u.UserId != user.UserId)?.Nickname ?? "Abandoned Chat",
                _ => throw new ArgumentException("Invalid value")
            },
            Users = conversation.Users.Select(user => new UserModel
            {
                Id = user.UserId,
                Name = user.Nickname
            }).ToList(),
            MessageCount = conversation.Messages.Count,
            LastMessage = conversation.Messages.FirstOrDefault()?.TimeStamp
        }));
    }
}
