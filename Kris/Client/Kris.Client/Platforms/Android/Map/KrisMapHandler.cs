using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics.Drawables;
using Kris.Client.Behaviors.Map;
using Kris.Client.Platforms.Callbacks;
using Kris.Client.Platforms.Listeners;
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
            foreach (var marker in mapHandler.Markers)
            {
                marker.Remove();
            }

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
                if (pin is KrisPinBehavior cp)
                {
                    cp.ImageSource.LoadImage(MauiContext, result =>
                    {
                        if (result?.Value is BitmapDrawable bitmapDrawable)
                        {
                            markerOption.SetIcon(BitmapDescriptorFactory.FromBitmap(bitmapDrawable.Bitmap));
                        }

                        var marker = NativeMap.AddMarker(markerOption);
                        pin.MarkerId = marker.Id;
                        Markers.Add(marker);
                    });
                }
                else
                {
                    var marker = NativeMap.AddMarker(markerOption);
                    pin.MarkerId = marker.Id;
                    Markers.Add(marker);
                }
            }
        }
    }
}
