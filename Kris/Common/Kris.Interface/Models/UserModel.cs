namespace Kris.Interface.Models;

public class UserModel
{
    public required Guid Id { get; set; }
    public string? Login { get; set; }
    public string? Nickname { get; set; }
}
