﻿using Kris.Common.Enums;
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

        switch (conversation.ConversationType)
        {
            case ConversationType.Direct:
                conversation.Name = string.Join(' ', conversation.Users.Select(user => user.Name));
                break;
            case ConversationType.Global:
                conversation.Name = "Global chat";
                break;
            default:
                conversation.Name = "Chat";
                break;
        }

        return conversation;
    }
}