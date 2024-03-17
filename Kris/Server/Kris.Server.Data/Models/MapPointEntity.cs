using Kris.Common.Models;

namespace Kris.Server.Data.Models;

public sealed class MapPointEntity : MapObjectEntity
{
    public required GeoPosition Position { get; set; }
    public required MapPointSymbol Symbol { get; set; }
    public List<MapPointAttachmentEntity> Attachments { get; set; } = new List<MapPointAttachmentEntity>();
}
