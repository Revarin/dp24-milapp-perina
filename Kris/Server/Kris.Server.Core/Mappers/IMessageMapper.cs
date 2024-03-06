using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public interface IMessageMapper
{
    MessageModel Map(MessageEntity entity);
}
