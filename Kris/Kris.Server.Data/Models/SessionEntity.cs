namespace Kris.Server.Data.Models;

public class SessionEntity : EntityBase
{
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required DateTime Created { get; set; }
    public List<SessionUserEntity> Users { get; set; } = new List<SessionUserEntity>();
}
