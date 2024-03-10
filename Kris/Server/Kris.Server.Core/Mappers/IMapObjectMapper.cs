using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public interface IMapObjectMapper
{
    MapPointListModel MapPointList(MapPointEntity entity);
    MapPointDetailModel MapPointDetail(MapPointEntity entity);
}
