using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Java.Lang;
using Kris.Client.Platforms.Callbacks;
using Kris.Client.Platforms.Listeners;
using Kris.Client.Platforms.Map;
using Kris.Client.Platforms.Utility;
using Kris.Common.Enums;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Platform;

namespace Kris.Client.Components.Map;

public partial class KrisMapHandler
{
    public List<Marker> Markers { get; } = new();

    private readonly MapLongClickListener _mapLongClickListener = new();
    private TileOverlay _tileOverlay;

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
            mapHandler.UpdatePins(map.Pins, map.KrisMapStyle);
        }
    }

    private static void MapKrisMapStyle(IKrisMapHandler handler, IKrisMap map)
    {
        if (handler is KrisMapHandler mapHandler)
        {
            mapHandler.SetStyle(map.KrisMapStyle);
        }
    }

    private void UpdatePins(IEnumerable<IMapPin> mapPins, KrisMapStyle style)
    {
        if (NativeMap is null || MauiContext is null) return;

        var confirmedMarkers = new List<string>();

        foreach (var pin in mapPins)
        {
            var pinHandler = pin.ToHandler(MauiContext);
            if (pinHandler is IMapPinHandler mapPinHandler)
            {
                if (pin is IKrisMapPin krisPin)
                {
                    if (!krisPin.Updated)
                    {
                        confirmedMarkers.Add(krisPin.MarkerId as string);
                    }
                    else
                    {
                        var markerOption = mapPinHandler.PlatformView;
                        Bitmap bitmap;

                        if (krisPin.KrisType == Common.Enums.KrisPinType.Self)
                        {
                            bitmap = PinIconDrawer.DrawImage(krisPin.ImageName, 1.0f, Context);
                        }
                        else
                        {
                            bitmap = PinIconDrawer.DrawImageWithLabel(krisPin.ImageName, krisPin.Label, 1.5f, Context);
                        }

                        var bitmapDesc = BitmapDescriptorFactory.FromBitmap(bitmap);
                        markerOption.SetIcon(bitmapDesc);
                        var marker = NativeMap.AddMarker(markerOption);
                        if (krisPin.KrisType == Common.Enums.KrisPinType.Self) marker.ZIndex = 999f;

                        krisPin.MarkerId = marker.Id;
                        krisPin.Updated = false;
                        confirmedMarkers.Add(krisPin.MarkerId as string);
                        Markers.Add(marker);
                    }
                }
            }
        }

        var deletedMarkers = Markers.ExceptBy(confirmedMarkers, m => m.Id);
        foreach (var dm in deletedMarkers)
        {
            dm.Remove();
        }
    }

    private void SetStyle(KrisMapStyle style)
    {
        if (NativeMap is null || MauiContext is null || style is null) return;

        NativeMap.ResetMinMaxZoomPreference();
        if (_tileOverlay != null) _tileOverlay.Remove();

        if (style.KrisMapType == KrisMapType.Satelite)
        {
            NativeMap.MapType = GoogleMap.MapTypeSatellite;
        }
        else if (style.KrisMapType == KrisMapType.Military)
        {
            NativeMap.MapType = GoogleMap.MapTypeNone;
            NativeMap.SetMapStyle(new MapStyleOptions(style.JsonStyle));

            var tileOverlayOptions = new TileOverlayOptions();
            var tileProvider = new KrisMilitaryTileProvider(style.TileSource);
            tileOverlayOptions.InvokeTileProvider(tileProvider);
            _tileOverlay = NativeMap.AddTileOverlay(tileOverlayOptions);

            NativeMap.SetMaxZoomPreference(15.9f);
            NativeMap.SetMinZoomPreference(13f);
        }
        else if (!string.IsNullOrEmpty(style.JsonStyle))
        {
            NativeMap.MapType = GoogleMap.MapTypeNormal;
            NativeMap.SetMapStyle(new MapStyleOptions(style.JsonStyle));
        }
    }
}
