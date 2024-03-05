﻿using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Conversation;

public sealed class DeleteConversationCommandHandler : ConversationHandler, IRequestHandler<DeleteConversationCommand, Result>
{
    private readonly IAuthorizationService _authorizationService;

    public DeleteConversationCommandHandler(IAuthorizationService authorizationService,
        IConversationRepository conversationRepository)
        : base(conversationRepository)
    {
        _authorizationService = authorizationService;
    }

    public async Task<Result> Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        var authResult = await _authorizationService.AuthorizeAsync(user, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var conversation = await _conversationRepository.GetWithAllAsync(request.ConversationId, cancellationToken);
        if (conversation == null) return Result.Fail(new EntityNotFoundError("conversation", request.ConversationId));

        if (conversation.Users.Count > 1) return Result.Fail(new InvalidOperationError("Cannot delete conversation with multiple users"));

        if (conversation.Users.Count == 0)
        {
            await _conversationRepository.DeleteAsync(conversation, cancellationToken);
        }
        else
        {
            var conversationUser = conversation.Users.First();
            if (conversationUser.UserId != user.UserId || conversationUser.Id != authResult.SessionUserId)
                return Result.Fail(new UnauthorizedError("Cannot delete conversation you are not participating"));

            await _conversationRepository.DeleteAsync(conversation, cancellationToken);
        }

        return Result.Ok();
    }
}
