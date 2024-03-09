using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Conversation;

public sealed class DeleteConversationCommandHandler : ConversationHandler, IRequestHandler<DeleteConversationCommand, Result>
{
    public DeleteConversationCommandHandler(IConversationRepository conversationRepository, IAuthorizationService authorizationService)
        : base(conversationRepository, authorizationService)
    {
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
            _conversationRepository.Delete(conversation);
            await _conversationRepository.UpdateAsync(cancellationToken);
        }
        else
        {
            var conversationUser = conversation.Users.First();
            if (conversationUser.UserId != user.UserId || conversationUser.Id != authResult.SessionUserId)
                return Result.Fail(new UnauthorizedError("Cannot delete conversation you are not participating"));

            _conversationRepository.Delete(conversation);
            await _conversationRepository.UpdateAsync(cancellationToken);
        }

        return Result.Ok();
    }
}
