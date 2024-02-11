using Kris.Server.Core.Mappers;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;

namespace Kris.Server.Core.Handlers.Position;

public abstract class PositionHandler : BaseHandler
{
    protected readonly IUserPositionRepository _positionRepository;
    protected readonly IPositionMapper _positionMapper;
    protected readonly IAuthorizationService _authorizationService;

    protected PositionHandler(IUserPositionRepository positionRepository, IPositionMapper positionMapper, IAuthorizationService authorizationService)
    {
        _positionRepository = positionRepository;
        _positionMapper = positionMapper;
        _authorizationService = authorizationService;
    }
}
