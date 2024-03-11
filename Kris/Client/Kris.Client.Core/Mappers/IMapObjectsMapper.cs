using InterfaceMapPointListModel = Kris.Interface.Models.MapPointListModel;
using InterfaceMapPointDetailModel = Kris.Interface.Models.MapPointDetailModel;
using ClientMapPointListModel = Kris.Client.Core.Models.MapPointListModel;
using ClientMapPointDetailModel = Kris.Client.Core.Models.MapPointDetailModel;

namespace Kris.Client.Core.Mappers;

public interface IMapObjectsMapper
{
    ClientMapPointListModel MapPoint(InterfaceMapPointListModel model);
    ClientMapPointDetailModel MapPoint(InterfaceMapPointDetailModel model);
}
