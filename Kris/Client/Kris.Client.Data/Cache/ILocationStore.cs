using Microsoft.Maui.Maps;

namespace Kris.Client.Data.Cache;

public interface ILocationStore
{
    void StoreCurrentRegion(MapSpan region);
    MapSpan GetCurrentRegion();
}
