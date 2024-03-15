using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Kris.Client.Platforms.Callbacks;
using Kris.Client.Platforms.Listeners;
using Kris.Client.Platforms.Utility;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Platform;

namespace Kris.Client.Components.Map;

public partial class KrisMapHandler
{
    public List<Marker> Markers { get; } = new();

    private readonly MapLongClickListener _mapLongClickListener = new();

    protected GoogleMap NativeMap { get; private set; }

    protected override async void ConnectHandler(MapView platformView)
    {
        base.ConnectHandler(platformView);

        var task = new TaskCompletionSource<GoogleMap>();
        platformView.GetMapAsync(new OnMapReadyCallback(task.SetResult));
        NativeMap = await task.Task;

        var nativeMap = NativeMap;
        if (nativeMap != null)
        {
            _mapLongClickListener.Handler = this;
            nativeMap.SetOnMapLongClickListener(_mapLongClickListener);
        }
    }

    protected override void DisconnectHandler(MapView platformView)
    {
        var nativeMap = NativeMap;
        if (nativeMap != null)
        {
            nativeMap.SetOnMapLongClickListener(null);
        }

        base.DisconnectHandler(platformView);
    }

    // Source: https://vladislavantonyuk.github.io/articles/Customize-map-pins-in-.NET-MAUI/
    private static void MapPins(IKrisMapHandler handler, IKrisMap map)
    {
        if (handler is KrisMapHandler mapHandler)
        {
            // TODO: Rework, keep removing and adding pin
            mapHandler.Markers.ForEach(marker => marker.Remove());
            mapHandler.Markers.Clear();

            mapHandler.AddPins(map.Pins);
        }
    }

    // Source: https://vladislavantonyuk.github.io/articles/Customize-map-pins-in-.NET-MAUI/
    private void AddPins(IEnumerable<IMapPin> mapPins)
    {
        if (NativeMap is null || MauiContext is null) return;

        foreach (var pin in mapPins)
        {
            var pinHandler = pin.ToHandler(MauiContext);
            if (pinHandler is IMapPinHandler mapPinHandler)
            {
                var markerOption = mapPinHandler.PlatformView;
                if (pin is IKrisMapPin krisPin)
                {
                    var bitmap = PinIconDrawer.DrawImageWithLabel(krisPin.ImageName, krisPin.Label, Context);
                    var bitmapDesc = BitmapDescriptorFactory.FromBitmap(bitmap);
                    markerOption.SetIcon(bitmapDesc);

                    var marker = NativeMap.AddMarker(markerOption);
                    pin.MarkerId = marker.Id;
                    Markers.Add(marker);
                }
            }
        }
    }

    private static void MapKrisMapStyle(IKrisMapHandler handler, IKrisMap map)
    {
        if (handler.Map != null)
        {
            var mapStyleOptions = map.KrisMapStyle != null ? new MapStyleOptions(map.KrisMapStyle.JsonStyle) : null;
            handler.Map.SetMapStyle(mapStyleOptions);
        }
    }
}
