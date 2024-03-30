using ClientSessionListModel = Kris.Client.Core.Models.SessionListModel;
using InterfaceSessionListModel = Kris.Interface.Models.SessionListModel;

namespace Kris.Client.Core.Mappers;

public interface ISessionMapper
{
    ClientSessionListModel Map(InterfaceSessionListModel model);
}
