using Kris.Client.Common.Enums;
using Microsoft.Maui.Maps;

namespace Kris.Client.Components.Map;

public interface IKrisMapPin : IMapPin
{
    Guid KrisId { get; set; }
    KrisPinType KrisType { get; set; }
    string ImageName { get; set; }
}
