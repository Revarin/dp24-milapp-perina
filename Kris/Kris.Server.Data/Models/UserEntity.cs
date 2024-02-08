namespace Kris.Server.Data.Models;

public class UserEntity : EntityBase
{
    public required string Login { get; set; }
    public string? Password { get; set; }
    public required DateTime Created { get; set; }
}
