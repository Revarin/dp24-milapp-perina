using Kris.Common.Models;

namespace Kris.Interface.Requests;

public sealed class SavePositionRequest : RequestBase
{
    public required GeoSpatialPosition Position { get; set; }
}
