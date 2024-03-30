using Kris.Client.Common.Options;
using Kris.Client.Data.Models;
using Kris.Common.Models;
using Microsoft.Extensions.Options;

namespace Kris.Client.Data.Cache;

public sealed class IdentityStore : StoreBase, IIdentityStore
{
    private const string IdentityKey = "kris-user-identity";
    private const string TokenKey = "kris-user-jwttoken";
    private const string LoginExpirationKey = "kris-user-expiration";
    private const string JoinedSessionsKey = "kris-user-sessions";

    private readonly SettingsOptions _settings;

    public IdentityStore(IOptions<SettingsOptions> options)
    {
        _settings = options.Value;
    }

    public void StoreIdentity(UserIdentityTokenEntity identity)
    {
        UserIdentityEntity user = identity;
        Set(IdentityKey, user);
        Set(TokenKey, identity.Token);
        Set(JoinedSessionsKey, identity.JoinedSessions);
        Set(LoginExpirationKey, DateTime.UtcNow.AddMinutes(_settings.LoginExpirationMinutes));
    }

    public UserIdentityEntity GetIdentity()
    {
        return Get<UserIdentityEntity>(IdentityKey);
    }

    public DateTime GetLoginExpiration()
    {
        return Get<DateTime>(LoginExpirationKey);
    }

    public JwtToken GetJwtToken()
    {
        return new JwtToken(Get<string>(TokenKey));
    }

    public IEnumerable<Guid> GetJoinedSessions()
    {
        return Get<IEnumerable<Guid>>(JoinedSessionsKey);
    }

    public void ClearIdentity()
    {
        Remove(IdentityKey);
        Remove(TokenKey);
        Remove(JoinedSessionsKey);
        Remove(LoginExpirationKey);
    }

    public void ClearCurrentSession()
    {
        var user = GetIdentity();
        var joinedSession = GetJoinedSessions().ToList();

        if (user.CurrentSession != null)
        {
            joinedSession.Remove(user.CurrentSession.Id);
            user.CurrentSession = null;
        }

        Set(IdentityKey, user);
        Set(JoinedSessionsKey, joinedSession);
    }
}
