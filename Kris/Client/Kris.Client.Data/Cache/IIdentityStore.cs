using Kris.Client.Data.Models;
using Kris.Common.Models;

namespace Kris.Client.Data.Cache;

public interface IIdentityStore
{
    void StoreIdentity(UserIdentityTokenEntity identity);
    UserIdentityEntity GetIdentity();
    JwtToken GetJwtToken();
    void ClearIdentity();
}
