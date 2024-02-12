using Kris.Server.Core.Mappers;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;

namespace Kris.Server.Core.Handlers.Session;

public abstract class SessionHandler : BaseHandler
{
    protected readonly ISessionRepository _sessionRepository;
    protected readonly ISessionMapper _sessionMapper;
    protected readonly IAuthorizationService _authorizationService;

    protected SessionHandler(ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
    {
        _sessionRepository = sessionRepository;
        _sessionMapper = sessionMapper;
        _authorizationService = authorizationService;
    }
}
