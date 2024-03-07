
namespace Kris.Client.Core.Mappers;

public class MessageMapper : IMessageMapper
{
    public Models.MessageModel Map(Interface.Models.MessageModel model)
    {
        return new Models.MessageModel
        {
            Id = model.Id,
            SenderId = model.SenderId,
            SenderName = model.SenderName,
            Body = model.Body,
            TimeStamp = model.TimeStamp,
        };
    }
}
