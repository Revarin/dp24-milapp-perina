using FluentResults;
using Kris.Common.Enums;
using Kris.Server.Core.Requests;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Conversation;

public sealed class RemoveEmptyConversationsCommandHandler : ConversationHandler, IRequestHandler<RemoveEmptyConversationsCommand, Result>
{

    public RemoveEmptyConversationsCommandHandler(IConversationRepository conversationRepository)
        : base(conversationRepository)
    {
    }

    public async Task<Result> Handle(RemoveEmptyConversationsCommand request, CancellationToken cancellationToken)
    {
        // Authentized from previous command
        // WARNING: Must not be accessible from API
        var conversations = await _conversationRepository.GetInSessionAsync(request.SessionId, cancellationToken);

        foreach (var conversation in conversations)
        {
            if (conversation.ConversationType == ConversationType.Direct
                && conversation.Users.Count <= 1
                && conversation.Messages.Count == 0)
            {
                await _conversationRepository.DeleteAsync(conversation, cancellationToken);
            }
        }

        return Result.Ok();
    }
}
