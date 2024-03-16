using Kris.Common.Enums;
using Kris.Common.Models;
using Kris.Interface.Models;

namespace Kris.Interface.Requests;

public sealed class EditMapPointRequest : RequestBase
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public MapObjectType Type { get; set; }
    public required GeoPosition Position { get; set; }
    public required MapPointSymbol Symbol { get; set; }
    public List<Guid> DeletedAttachments { get; set; } = new List<Guid>();
    public List<MapPointAttachmentModel> NewAttachments { get; set; } = new List<MapPointAttachmentModel>();
}
