namespace Kris.Server.Core.Models;

public sealed class UserModel
{
    public required Guid Id { get; set; }
    public required string Login { get; set; }
}
