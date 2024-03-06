using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public class MessageMapper : IMessageMapper
{
    public MessageModel Map(MessageEntity entity)
    {
        return new MessageModel
        {
            Id = entity.Id,
            SenderId = entity.SenderId,
            SenderName = entity.Sender?.User?.Login,
            Body = entity.Body,
            TimeStamp = entity.TimeStamp
        };
    }
}
