using Kris.Server.Core.Services;

namespace Kris.Server.Core.Handlers.MapObject;

public abstract class MapObjectHandler
{
    protected readonly IAuthorizationService _authorizationService;

    protected MapObjectHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }
}
