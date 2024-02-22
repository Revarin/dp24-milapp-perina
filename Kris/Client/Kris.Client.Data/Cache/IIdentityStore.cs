using Kris.Client.Data.Models;
using Kris.Common.Models;

namespace Kris.Client.Data.Cache;

public interface IIdentityStore
{
    void StoreIdentity(UserIdentityTokenEntity identity);
    UserIdentityEntity GetIdentity();
    DateTime GetLoginExpiration();
    JwtToken GetJwtToken();
    IEnumerable<Guid> GetJoinedSessions();
    void ClearIdentity();
    void ClearCurrentSession();
}
