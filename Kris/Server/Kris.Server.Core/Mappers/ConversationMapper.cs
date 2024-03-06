using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public sealed class ConversationMapper : IConversationMapper
{
    public ConversationListModel Map(ConversationEntity entity)
    {
        var conversation = new ConversationListModel
        {
            Id = entity.Id,
            ConversationType = entity.ConversationType,
            MessageCount = entity.Messages.Count,
            LastMessage = entity.Messages.FirstOrDefault()?.TimeStamp,
            Users = entity.Users.Select(user => new UserModel
            {
                Id = user.UserId,
                Name = user.User!.Login
            }).ToList()
        };
        conversation.Name = string.Join(' ', conversation.Users.Select(user => user.Name));
        return conversation;
    }
}
