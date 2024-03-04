using Kris.Client.Core.Models;
using Kris.Client.ViewModels.Views;

namespace Kris.Client.Utility;

public interface IKrisMapObjectFactory
{
    KrisMapPinViewModel CreateMyPositionPin(Guid userId, string userName, Location location);
    KrisMapPinViewModel CreateUserPositionPin(UserPositionModel userPosition);
    KrisMapPinViewModel CreateMapPoint(MapPointModel mapPoint);
}
