namespace Kris.Interface.Requests;

public sealed class EditUserRequest : PasswordRequest
{
    public required string NewPassword { get; set; }
}
