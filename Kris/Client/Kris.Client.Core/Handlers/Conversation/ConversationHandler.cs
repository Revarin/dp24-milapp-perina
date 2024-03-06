using Kris.Interface.Controllers;

namespace Kris.Client.Core.Handlers.Conversation;

public abstract class ConversationHandler : BaseHandler
{
    protected readonly IConversationController _conversationClient;

    protected ConversationHandler(IConversationController conversationClient)
    {
        _conversationClient = conversationClient;
    }
}
