using InterfaceMessageModel = Kris.Interface.Models.MessageModel;
using ClientMessageModel = Kris.Client.Core.Models.MessageModel;

namespace Kris.Client.Core.Mappers;

public interface IMessageMapper
{
    ClientMessageModel Map(InterfaceMessageModel model);
}
