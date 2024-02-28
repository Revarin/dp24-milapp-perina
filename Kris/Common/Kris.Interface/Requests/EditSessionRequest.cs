namespace Kris.Interface.Requests;

public sealed class EditSessionRequest : PasswordRequest
{
    public required string NewName { get; set; }
    public required string NewPassword { get; set; }
}
