using Kris.Common.Models;

using InterfaceUserPositionModel = Kris.Interface.Models.UserPositionModel;
using ClientUserPositionModel = Kris.Client.Core.Models.UserPositionModel;

namespace Kris.Client.Core.Mappers;

public interface IPositionMapper
{
    GeoSpatialPosition Map(Location location);
    Location Map(GeoSpatialPosition position);
    ClientUserPositionModel Map(InterfaceUserPositionModel position);
}
