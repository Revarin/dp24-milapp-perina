namespace Kris.Interface.Requests;

public sealed class JoinSessionRequest : RequestBase
{
    public required Guid Id { get; set; }
    public required string Password { get; set; }
}
