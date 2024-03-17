using Kris.Common.Enums;
using Kris.Common.Models;
using Kris.Interface.Models;

namespace Kris.Interface.Requests;

public sealed class AddMapPointRequest : RequestBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public MapObjectType Type { get; set; }
    public required GeoPosition Position { get; set; }
    public required MapPointSymbol Symbol { get; set; }
    public List<MapPointAttachmentModel> Attachments { get; set; } = new List<MapPointAttachmentModel>();
}
