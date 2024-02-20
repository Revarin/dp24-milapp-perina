using Kris.Interface.Controllers;

namespace Kris.Client.Core.Handlers.Session;

public abstract class SessionHandler : BaseHandler
{
    protected readonly ISessionController _sessionClient;

    protected SessionHandler(ISessionController sessionClient)
    {
        _sessionClient = sessionClient;
    }
}
