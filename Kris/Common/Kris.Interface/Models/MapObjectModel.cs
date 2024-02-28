using Kris.Common.Enums;

namespace Kris.Interface.Models;

public class MapObjectModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required MapObjectType Type { get; set; }
    public string? Description { get; set; }
    public required DateTime Created { get; set; }
    public required UserModel User { get; set; }
}
