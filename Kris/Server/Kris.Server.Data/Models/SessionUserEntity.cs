using Kris.Common.Enums;

namespace Kris.Server.Data.Models;

public class SessionUserEntity : EntityBase
{
    public required Guid SessionId { get; set; }
    public SessionEntity? Session { get; set; }
    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    public required UserType UserType { get; set; }
    public required DateTime Joined { get; set; }

    public List<MapPointEntity> MapPoints { get; set; } = new List<MapPointEntity>();
}
