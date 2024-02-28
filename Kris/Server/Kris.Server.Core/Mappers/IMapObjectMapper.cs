using Kris.Interface.Models;
using Kris.Server.Data.Models;

namespace Kris.Server.Core.Mappers;

public interface IMapObjectMapper
{
    MapPointModel MapPoint(MapPointEntity entity);
}
