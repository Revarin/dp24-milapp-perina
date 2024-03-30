using Kris.Client.Common.Enums;
using Kris.Client.Core.Models;
using Kris.Client.ViewModels.Views;

namespace Kris.Client.Utility;

public interface IKrisMapObjectFactory
{
    KrisMapPinViewModel CreateUserPositionPin(UserPositionModel userPosition, KrisPinType krisPinType);
    KrisMapPinViewModel CreateMapPoint(MapPointListModel mapPoint);
}
