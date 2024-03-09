using FluentResults;
using Kris.Common.Enums;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Conversation;

public sealed class JoinConversationsCommandHandler : ConversationHandler, IRequestHandler<JoinConversationsCommand, Result>
{
    private readonly ISessionRepository _sessionRepository;

    public JoinConversationsCommandHandler(ISessionRepository sessionRepository,
        IConversationRepository conversationRepository, IAuthorizationService authorizationService)
        : base(conversationRepository, authorizationService)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Result> Handle(JoinConversationsCommand request, CancellationToken cancellationToken)
    {
        // Authentized from previous command
        // WARNING: Must not be accessible from API
        var session = await _sessionRepository.GetWithConversations(request.SessionId, cancellationToken);
        if (session == null) return Result.Fail(new EntityNotFoundError("Session", request.SessionId));

        var currentUser = session.Users.Find(sessionUser => sessionUser.UserId == request.UserId);
        if (currentUser == null) throw new DatabaseException("User not in session");

        // Join global conversations
        var globalConversation = session.Conversations.Find(conversation => conversation.ConversationType == ConversationType.Global);
        if (globalConversation == null) throw new DatabaseException("Global conversation is missing");
        if (!globalConversation.Users.Contains(currentUser))
        {
            globalConversation.Users.Add(currentUser);
        }

        // Create direct conversations with all users
        foreach (var otherUser in session.Users)
        {
            if (otherUser.Id == currentUser.Id) continue;

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
