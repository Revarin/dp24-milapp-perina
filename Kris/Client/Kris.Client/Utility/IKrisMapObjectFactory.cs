using Kris.Client.Components.Map;
using Kris.Client.Core.Models;

namespace Kris.Client.Utility;

public interface IKrisMapObjectFactory
{
    KrisMapPin CreateMyPositionPin(Guid userId, string userName, Location location);
    KrisMapPin CreateUserPositionPin(UserPositionModel userPosition);
    KrisMapPin CreateMapPoint(MapPointModel mapPoint);
}
