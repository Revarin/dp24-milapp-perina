using Android.Gms.Maps;

using IMap = Microsoft.Maui.Maps.IMap;

namespace Kris.Client.Platforms.Map;

// Source: https://vladislavantonyuk.github.io/articles/Customize-map-pins-in-.NET-MAUI/
public class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
{
    private readonly CustomMapHandler mapHandler;

    public MapCallbackHandler(CustomMapHandler mapHandler)
    {
        this.mapHandler = mapHandler;
    }

    public void OnMapReady(GoogleMap googleMap)
    {
        mapHandler.UpdateValue(nameof(IMap.Pins));
    }
}