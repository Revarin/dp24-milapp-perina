using InterfaceMapPointListModel = Kris.Interface.Models.MapPointListModel;
using ClientMapPointListModel = Kris.Client.Core.Models.MapPointListModel;

namespace Kris.Client.Core.Mappers;

public interface IMapObjectsMapper
{
    ClientMapPointListModel Map(InterfaceMapPointListModel model);
}
