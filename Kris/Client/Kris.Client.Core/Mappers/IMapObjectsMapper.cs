using InterfaceMapPointModel = Kris.Interface.Models.MapPointModel;
using ClientMapPointModel = Kris.Client.Core.Models.MapPointModel;

namespace Kris.Client.Core.Mappers;

public interface IMapObjectsMapper
{
    ClientMapPointModel Map(InterfaceMapPointModel model);
}
