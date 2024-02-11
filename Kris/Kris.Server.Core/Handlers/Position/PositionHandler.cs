using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;

namespace Kris.Server.Core.Handlers.Position;

public abstract class PositionHandler : BaseHandler
{
    protected readonly IUserPositionRepository _positionRepository;
    protected readonly IAuthorizationService _authorizationService;

    protected PositionHandler(IUserPositionRepository positionRepository, IAuthorizationService authorizationService)
    {
        _positionRepository = positionRepository;
        _authorizationService = authorizationService;
    }
}
