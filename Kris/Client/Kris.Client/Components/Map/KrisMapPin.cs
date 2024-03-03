using Kris.Client.Common.Enums;
using Kris.Client.Core.Models;

namespace Kris.Client.Components.Map;

public class KrisMapPin
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Location Location { get; set; }
    public DateTime TimeStamp { get; set; }
    public KrisPinType PinType { get; set; }
    public ImageSource ImageSource { get; set; }

    public KrisMapPin()
    {
    }
}
