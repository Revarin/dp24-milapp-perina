namespace Kris.Interface.Requests;

public sealed class EditSessionRequest : RequestBase
{
    public required string Name { get; set; }
    public required string Password { get; set; }
}
