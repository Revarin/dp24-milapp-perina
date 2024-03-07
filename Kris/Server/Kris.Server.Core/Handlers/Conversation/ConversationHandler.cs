using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;

namespace Kris.Server.Core.Handlers.Conversation;

public abstract class ConversationHandler : BaseHandler
{
    protected readonly IConversationRepository _conversationRepository;
    protected readonly IAuthorizationService _authorizationService;

    protected ConversationHandler(IConversationRepository conversationRepository, IAuthorizationService authorizationService)
    {
        _conversationRepository = conversationRepository;
        _authorizationService = authorizationService;
    }
}
