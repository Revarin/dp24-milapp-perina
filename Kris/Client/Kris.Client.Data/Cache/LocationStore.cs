using Microsoft.Maui.Maps;

namespace Kris.Client.Data.Cache;

public sealed class LocationStore : StoreBase, ILocationStore
{
    private const string LocationKey = "kris-location-location";
    private const string RadiusKey = "kris-location-radius";

    public void StoreCurrentRegion(MapSpan region)
    {
        Set(LocationKey, region.Center);
        Set(RadiusKey, region.Radius.Meters);
    }

    public MapSpan GetCurrentRegion()
    {
        var center = Get<Location>(LocationKey);
        var radius = Get<double>(RadiusKey);
        if (center == null || radius == default) return null;

        return MapSpan.FromCenterAndRadius(center, Distance.FromMeters(radius));
    }
}
