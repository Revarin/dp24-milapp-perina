using Kris.Server.Core.Mappers;
using Kris.Server.Data.Repositories;

namespace Kris.Server.Core.Handlers.Session;

public abstract class SessionHandler : BaseHandler
{
    protected readonly ISessionRepository _sessionRepository;
    protected readonly ISessionMapper _sessionMapper;

    protected SessionHandler(ISessionRepository sessionRepository, ISessionMapper sessionMapper)
    {
        _sessionRepository = sessionRepository;
        _sessionMapper = sessionMapper;
    }
}
