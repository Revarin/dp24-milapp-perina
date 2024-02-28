namespace Kris.Interface.Requests;

public sealed class CreateSessionRequest : RequestBase
{
    public required string Name { get; set; }
    public required string Password { get; set; }
}
