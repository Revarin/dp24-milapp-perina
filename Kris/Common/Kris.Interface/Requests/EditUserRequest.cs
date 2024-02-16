namespace Kris.Interface.Requests;

public sealed class EditUserRequest : RequestBase
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}
