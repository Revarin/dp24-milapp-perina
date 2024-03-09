﻿using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using MediatR;

namespace Kris.Client.Core.Handlers.Conversation;

public sealed class DeleteConversationCommandHandler : ConversationHandler, IRequestHandler<DeleteConversationCommand, Result>
{
    public DeleteConversationCommandHandler(IConversationController conversationClient)
        : base(conversationClient)
    {
    }

    public async Task<Result> Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
    {
        var response = await _conversationClient.DeleteConversation(request.ConversationId, cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsForbidden()) return Result.Fail(new ForbiddenError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok();
    }
}
