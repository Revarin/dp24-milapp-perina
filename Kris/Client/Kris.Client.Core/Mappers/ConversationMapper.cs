
namespace Kris.Client.Core.Mappers;

public sealed class ConversationMapper : IConversationMapper
{
    public Models.ConversationModel Map(Interface.Models.ConversationListModel model)
    {
        return new Models.ConversationModel
        {
            Id = model.Id,
            ConversationType = model.ConversationType,
            MessageCount = model.MessageCount,
            LastMessage = model.LastMessage
        };
    }
}
