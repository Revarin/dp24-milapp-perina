﻿using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Kris.Client.Platforms.Callbacks;
using Kris.Client.Platforms.Listeners;
using Kris.Client.Platforms.Map;
using Kris.Client.Platforms.Utility;
using Kris.Common.Enums;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Platform;

using Color = Android.Graphics.Color;

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

            mapHandler.AddPins(map.Pins, map.KrisMapStyle);
        }
    }

    private static void MapKrisMapStyle(IKrisMapHandler handler, IKrisMap map)
    {
        if (handler is KrisMapHandler mapHandler)
        {
            mapHandler.SetStyle(map.KrisMapStyle);
        }
    }

    // Source: https://vladislavantonyuk.github.io/articles/Customize-map-pins-in-.NET-MAUI/
    private void AddPins(IEnumerable<IMapPin> mapPins, KrisMapStyle style)
    {
        if (NativeMap is null || MauiContext is null) return;

        var textColor = style.KrisMapType == KrisMapType.StreetLight || style.KrisMapType == KrisMapType.Custom ? Color.Black : Color.White;

        foreach (var pin in mapPins)
        {
            var pinHandler = pin.ToHandler(MauiContext);
            if (pinHandler is IMapPinHandler mapPinHandler)
            {
                var markerOption = mapPinHandler.PlatformView;
                if (pin is IKrisMapPin krisPin)
                {
                    var bitmap = PinIconDrawer.DrawImageWithLabel(krisPin.ImageName, krisPin.Label, textColor, Context);
                    var bitmapDesc = BitmapDescriptorFactory.FromBitmap(bitmap);
                    markerOption.SetIcon(bitmapDesc);

                    var marker = NativeMap.AddMarker(markerOption);
                    pin.MarkerId = marker.Id;
                    Markers.Add(marker);
                }
            }
        }
    }

    private void SetStyle(KrisMapStyle style)
    {
        if (NativeMap is null || MauiContext is null) return;

        //NativeMap.AddTileOverlay(null);
        NativeMap.ResetMinMaxZoomPreference();

        if (style.KrisMapType == KrisMapType.Satelite)
        {
            NativeMap.MapType = GoogleMap.MapTypeSatellite;
        }
        else if (style.KrisMapType == KrisMapType.Custom)
        {
            NativeMap.MapType = GoogleMap.MapTypeNone;
            var tileOverlayOptions = new TileOverlayOptions();
            var tileProvider = new KrisTileProvider(style.TileSource);
            tileOverlayOptions.InvokeTileProvider(tileProvider);
            NativeMap.AddTileOverlay(tileOverlayOptions);

            NativeMap.SetMaxZoomPreference(15.9f);
            NativeMap.SetMinZoomPreference(15f);
        }
        else if (!string.IsNullOrEmpty(style.JsonStyle))
        {
            NativeMap.MapType = GoogleMap.MapTypeNormal;
            NativeMap.SetMapStyle(new MapStyleOptions(style.JsonStyle));
        }
    }
}
