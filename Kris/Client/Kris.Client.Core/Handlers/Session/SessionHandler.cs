using Kris.Client.Core.Mappers;
using Kris.Interface.Controllers;

namespace Kris.Client.Core.Handlers.Session;

public abstract class SessionHandler : BaseHandler
{
    protected readonly ISessionController _sessionClient;
    protected readonly IUserMapper _userMapper;

    protected SessionHandler(ISessionController sessionClient, IUserMapper userMapper)
    {
        _sessionClient = sessionClient;
        _userMapper = userMapper;
    }
}
