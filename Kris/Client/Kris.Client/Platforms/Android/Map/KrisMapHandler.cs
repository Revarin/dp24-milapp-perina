using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using AndroidX.ConstraintLayout.Core.Motion.Utils;
using Java.IO;
using Java.Lang;
using Kris.Client.Platforms.Android.Utility;
using Kris.Client.Platforms.Callbacks;
using Kris.Client.Platforms.Listeners;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Platform;
using static System.Net.Mime.MediaTypeNames;
using File = Java.IO.File;

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
                if (pin is KrisMPin krisPin)
                {
                    //var iconView = krisPin.View.Invoke();
                    //var nativeView = iconView.ToPlatform(MauiContext);
                    //var bitmapDescriptor = BitmapUtilities.ConvertViewToBitmapDescriptor(nativeView);
                    //markerOption.SetIcon(bitmapDescriptor);

                    if (krisPin.KrisType == Common.Enums.KrisPinType.Point)
                    {
                        //var imageBitmapDesc = BitmapDescriptorFactory.FromBitmap(imageBitmap);
                        //markerOption.SetIcon(imageBitmapDesc);

                        // Text
                        var paint = new Android.Graphics.Paint
                        {
                            AntiAlias = true,
                            Color = Android.Graphics.Color.Black,
                            TextSize = (Context.Resources.DisplayMetrics.Density * 14f) + 0.5f,
                            TextAlign = Android.Graphics.Paint.Align.Left
                        };
                        var baseline = -paint.Ascent();
                        var textWidth = (int)(paint.MeasureText(krisPin.Label));
                        var textHeight = (int)(baseline + paint.Descent());
                        //var textBitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
                        //var canvas = new Canvas(textBitmap);
                        //canvas.DrawRGB(255, 0, 0);
                        //canvas.DrawText(krisPin.Label, 0, baseline, paint);

                        // Image
                        var imageFile = new File(Context.CacheDir, krisPin.ImageName);
                        var imageBitmap = BitmapFactory.DecodeFile(imageFile.AbsolutePath);

                        // Combine
                        var finalHeight = textHeight + imageBitmap.Height;
                        var finalWidth = System.Math.Max(textWidth, imageBitmap.Width);
                        var imageLeft = (finalWidth / 2) - (imageBitmap.Width / 2);
                        var finalBitmap = Bitmap.CreateBitmap(finalWidth, finalHeight, Bitmap.Config.Argb8888);
                        var canvas = new Canvas(finalBitmap);
                        canvas.DrawRGB(200, 200, 200);
                        //var scaleMatrix = new Matrix();
                        //scaleMatrix.SetScale(1.2f, 1.2f);
                        //canvas.DrawBitmap(imageBitmap, scaleMatrix, null);
                        canvas.DrawText(krisPin.Label, 0, baseline, paint);
                        canvas.DrawBitmap(imageBitmap, imageLeft, textHeight, null);

                        var finalBitmapDesc = BitmapDescriptorFactory.FromBitmap(finalBitmap);
                        markerOption.SetIcon(finalBitmapDesc);
                    }
                    else
                    {
                        var labelView = new Label { Text = krisPin.Label, TextColor = Microsoft.Maui.Graphics.Color.FromRgb(0,0,0) };
                        var labelNative = labelView.ToPlatform(MauiContext);
                        var labelBitmapDesc = BitmapUtilities.ConvertViewToBitmapDescriptor(labelNative);
                        markerOption.SetIcon(labelBitmapDesc);
                    }

                    var marker = NativeMap.AddMarker(markerOption);
                    pin.MarkerId = marker.Id;
                    Markers.Add(marker);
                }
            }
        }
    }
}
