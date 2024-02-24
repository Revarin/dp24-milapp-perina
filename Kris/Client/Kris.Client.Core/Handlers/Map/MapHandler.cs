using Kris.Client.Data.Cache;

namespace Kris.Client.Core.Handlers.Map;

public abstract class MapHandler : BaseHandler
{
    protected ILocationStore _locationStore;

    protected MapHandler(ILocationStore locationStore)
    {
        _locationStore = locationStore;
    }
}
