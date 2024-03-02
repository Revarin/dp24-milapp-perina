using Kris.Client.Common.Enums;
using Kris.Client.Core.Models;

namespace Kris.Client.Components.Map;

public class KrisMapPin : KrisMapMarker
{
    public string Address { get; set; }
    public Location Location { get; set; }
    public ImageSource ImageSource { get; set; }
    public KrisPinType PinType { get; set; }
    public DateTime Updated { get; set; }

    public KrisMapPin()
    {
    }

    public KrisMapPin(UserPositionModel userPosition)
    {
        // TODO: Bad
        Id = userPosition.UserId;
        Name = userPosition.UserName;
        Updated = userPosition.Updated;
        Location = userPosition.Positions.First();
        PinType = KrisPinType.User;
        ImageSource = ImageSource.FromFile("point_blue.png");
    }

    public KrisMapPin(UserPositionModel userPosition, KrisPinType type) : this(userPosition)
    {
        // TODO: Bas
        PinType = type;
        ImageSource = ImageSource.FromFile("point_green.png");
    }
}
