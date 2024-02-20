namespace Kris.Client.Data.Models;

public sealed class UserIdentityTokenEntity : UserIdentityEntity
{
    public string Token { get; set; }
    public IEnumerable<Guid> JoinedSessions { get; set; }
}
