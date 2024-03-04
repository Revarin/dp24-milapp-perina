using Kris.Interface.Controllers;

namespace Kris.Client.Core.Handlers.MapObjects;

public abstract class MapObjectsHandler : BaseHandler
{
    protected readonly IMapObjectController _mapObjectClient;

    protected MapObjectsHandler(IMapObjectController mapObjectClient)
    {
        _mapObjectClient = mapObjectClient;
    }
}
