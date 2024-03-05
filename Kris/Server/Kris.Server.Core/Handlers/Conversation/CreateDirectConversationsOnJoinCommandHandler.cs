using FluentResults;
using Kris.Common.Enums;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Requests;
using Kris.Server.Data.Models;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Conversation;

public sealed class CreateDirectConversationsOnJoinCommandHandler : ConversationHandler, IRequestHandler<CreateDirectConversationsOnJoinCommand, Result>
{
    private readonly ISessionRepository _sessionRepository;

    public CreateDirectConversationsOnJoinCommandHandler(ISessionRepository sessionRepository,
        IConversationRepository conversationRepository)
        : base(conversationRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Result> Handle(CreateDirectConversationsOnJoinCommand request, CancellationToken cancellationToken)
    {
        // Authentized from previous command
        // WARNING: Must not be accessible from API

        var session = await _sessionRepository.GetWithAllAsync(request.JoinSession.Id, cancellationToken);
        if (session == null) return Result.Fail(new EntityNotFoundError("Session", request.JoinSession.Id));

        var currentUser = session.Users.Find(sessionUser => sessionUser.UserId == request.User.UserId);
        if (currentUser == null) throw new DatabaseException("User not in session");

        foreach (var otherUser in session.Users)
        {
            if (!session.Conversations.Any(conversation => 
                conversation.ConversationType == ConversationType.Direct
                && conversation.Users.Any(convUser => convUser.UserId == otherUser.UserId)
                && conversation.Users.Any(convUser => convUser.UserId == currentUser.UserId)))
            {
                var newConversation = new ConversationEntity
                {
                    ConversationType = ConversationType.Direct,
                    SessionId = session.Id,
                    Session = session,
                    Users = new List<SessionUserEntity>
                    {
                        currentUser,
                        otherUser
                    }
                };
                var entity = await _conversationRepository.InsertAsync(newConversation, cancellationToken);
                if (entity == null) throw new DatabaseException("Failed to insert conversation");
            }
        }

        return Result.Ok();
    }
}
