namespace Kris.Server.Data.Models;

public sealed class UserEntity : EntityBase
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required DateTime Created { get; set; }
}
