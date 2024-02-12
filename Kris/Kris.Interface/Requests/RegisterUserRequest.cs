namespace Kris.Interface.Requests;

public sealed class RegisterUserRequest : RequestBase
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}
