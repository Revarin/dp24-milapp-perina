using Android.Graphics;
using Android.Views;

namespace Kris.Client.Platforms.Android.Utility;

// Source: Maui.GoogleMaps
public static class BitmapUtilities
{
    public static Bitmap ConvertViewToBitmap(global::Android.Views.View androidView)
    {
        androidView.SetLayerType(LayerType.Hardware, null);
        androidView.DrawingCacheEnabled = true;

        androidView.Measure(
            global::Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
            global::Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
        androidView.Layout(0, 0, androidView.MeasuredWidth, androidView.MeasuredHeight);

        androidView.BuildDrawingCache(true);
        Bitmap bitmap = Bitmap.CreateBitmap(androidView.GetDrawingCache(true));
        androidView.DrawingCacheEnabled = false;
        return bitmap;
    }

    public static global::Android.Gms.Maps.Model.BitmapDescriptor ConvertViewToBitmapDescriptor(global::Android.Views.View androidView)
    {
        var bitmap = ConvertViewToBitmap(androidView);
        var bitmapDescriptor = global::Android.Gms.Maps.Model.BitmapDescriptorFactory.FromBitmap(bitmap);
        return bitmapDescriptor;
    }
}
