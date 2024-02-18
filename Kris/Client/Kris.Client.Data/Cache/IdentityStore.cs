using Kris.Client.Data.Models;
using Kris.Common.Models;

namespace Kris.Client.Data.Cache;

public sealed class IdentityStore : StoreBase, IIdentityStore
{
    private const string IdentityKey = "kris-user-identity";
    private const string TokenKey = "kris-user-jwttoken";

    public void StoreIdentity(UserIdentityTokenEntity identity)
    {
        UserIdentityEntity user = identity;
        Set(IdentityKey, user);
        Set(TokenKey, identity.Token);
    }

    public UserIdentityEntity GetIdentity()
    {
        return Get<UserIdentityEntity>(IdentityKey);
    }

    public JwtToken GetJwtToken()
    {
        return new JwtToken(Get<string>(TokenKey));
    }

    public void ClearIdentity()
    {
        Remove(IdentityKey);
        Remove(TokenKey);
    }
}
