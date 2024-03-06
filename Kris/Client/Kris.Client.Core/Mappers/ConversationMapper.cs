
namespace Kris.Client.Core.Mappers;

public sealed class ConversationMapper : IConversationMapper
{
    public Models.ConversationListModel Map(Interface.Models.ConversationListModel model)
    {
        return new Models.ConversationListModel
        {
            Id = model.Id,
            Name = model.Name,
            ConversationType = model.ConversationType,
            MessageCount = model.MessageCount,
            LastMessage = model.LastMessage
        };
    }
}
