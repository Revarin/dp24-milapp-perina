namespace Kris.Client.Core.Models;

public sealed class AvailableConversationsModel
{
    public IEnumerable<ConversationModel> SpecialConversations { get; set; }
    public IEnumerable<ConversationModel> DirectConversations { get; set; }
}
