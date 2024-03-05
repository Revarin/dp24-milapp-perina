using Kris.Server.Data.Repositories;

namespace Kris.Server.Core.Handlers.Conversation;

public abstract class ConversationHandler : BaseHandler
{
    protected readonly IConversationRepository _conversationRepository;

    protected ConversationHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }
}
