using Android.Content;
using Android.Graphics;

namespace Kris.Client.Platforms.Utility;

public static class PinIconDrawer
{
    // Source: https://stackoverflow.com/a/40192803
    public static Bitmap DrawImageWithLabel(string imageName, string label, Context context)
    {
        // Text
        var paint = new Android.Graphics.Paint
        {
            AntiAlias = true,
            Color = Android.Graphics.Color.WhiteSmoke,
            TextSize = (context.Resources.DisplayMetrics.Density * 14f) + 0.5f,
            TextAlign = Android.Graphics.Paint.Align.Left
        };
        var baseline = -paint.Ascent();
        var textWidth = (int)(paint.MeasureText(label));
        var textHeight = (int)(baseline + paint.Descent());

        // Image
        var imageFile = new Java.IO.File(context.CacheDir, imageName);
        var imageBitmap = BitmapFactory.DecodeFile(imageFile.AbsolutePath);

        // Combine
        var finalHeight = textHeight + imageBitmap.Height;
        var finalWidth = System.Math.Max(textWidth, imageBitmap.Width);
        var imageLeft = (finalWidth / 2) - (imageBitmap.Width / 2);
        var finalBitmap = Bitmap.CreateBitmap(finalWidth, finalHeight, Bitmap.Config.Argb8888);
        var canvas = new Canvas(finalBitmap);

        //var scaleMatrix = new Matrix();
        //scaleMatrix.SetScale(1.2f, 1.2f);
        //canvas.DrawBitmap(imageBitmap, scaleMatrix, null);

        canvas.DrawText(label, 0, baseline, paint);
        canvas.DrawBitmap(imageBitmap, imageLeft, textHeight, null);

        return finalBitmap;
    }
}
