using Kris.Client.Common.Enums;
using Kris.Client.Core.Models;

namespace Kris.Client.Components.Map;

public class MapPin : MapObject
{
    public string Address { get; set; }
    public Location Location { get; set; }
    public ImageSource ImageSource { get; set; }
    public KrisPinType PinType { get; set; }
    public DateTime Updated { get; set; }

    public MapPin()
    {
    }

    public MapPin(UserPositionModel userPosition)
    {
        Id = userPosition.UserId;
        Name = userPosition.UserName;
        Updated = userPosition.Updated;
        Location = userPosition.Positions.First();
        PinType = KrisPinType.User;
        ImageSource = ImageSource.FromFile("point_blue.png");
    }

    public MapPin(UserPositionModel userPosition, KrisPinType type) : this(userPosition)
    {
        PinType = type;
        ImageSource = ImageSource.FromFile("point_green.png");
    }
}
