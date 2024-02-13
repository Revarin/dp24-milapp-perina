using Kris.Common.Enums;

namespace Kris.Server.Data.Models;

public abstract class MapObjectEntity : EntityBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required DateTime Created { get; set; }
    public required MapObjectType Type { get; set; }
    public required Guid UserId { get; set; }
    public required Guid SessionId { get; set; }
    public SessionUserEntity? SessionUser { get; set; }
}
