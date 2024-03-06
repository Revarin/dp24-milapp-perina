using Kris.Client.Core.Mappers;
using Kris.Interface.Controllers;

namespace Kris.Client.Core.Handlers.Conversation;

public abstract class ConversationHandler : BaseHandler
{
    protected readonly IConversationController _conversationClient;
    protected readonly IConversationMapper _conversationMapper;

    protected ConversationHandler(IConversationController conversationClient, IConversationMapper conversationMapper)
    {
        _conversationClient = conversationClient;
        _conversationMapper = conversationMapper;
    }
}
