namespace Kris.Server.Data.Models;

public class UserEntity : EntityBase
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required DateTime Created { get; set; }
    public Guid? CurrentSessionId { get; set; }
    public SessionUserEntity? CurrentSession { get; set; }
    public List<SessionUserEntity> AllSessions { get; set; } = new List<SessionUserEntity>();
    public required UserSettingsEntity Settings { get; set; }
}
