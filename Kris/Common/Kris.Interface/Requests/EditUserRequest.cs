namespace Kris.Interface.Requests;

public sealed class EditUserRequest : RequestBase
{
    public required string NewLogin { get; set; }
    public required string NewPassword { get; set; }
    public required string Password { get; set; }
}
