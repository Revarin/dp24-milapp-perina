namespace Kris.Interface.Requests;

public sealed class JoinSessionRequest : RequestBase
{
    public required Guid SessionId { get; set; }
}
