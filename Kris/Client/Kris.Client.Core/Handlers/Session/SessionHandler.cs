using Kris.Client.Core.Mappers;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;

namespace Kris.Client.Core.Handlers.Session;

public abstract class SessionHandler : BaseHandler
{
    protected readonly ISessionController _sessionClient;
    protected readonly IIdentityStore _identityStore;
    protected readonly IUserMapper _userMapper;

    protected SessionHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
    {
        _sessionClient = sessionClient;
        _identityStore = identityStore;
        _userMapper = userMapper;
    }
}
