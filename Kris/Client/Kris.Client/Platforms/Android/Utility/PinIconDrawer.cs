using Android.Content;
using Android.Graphics;

using Color = Android.Graphics.Color;

namespace Kris.Client.Platforms.Utility;

public static class PinIconDrawer
{
    private const int StrokeWidth = 8;
    private const float TextSize = 14f;

    // Source: https://stackoverflow.com/a/40192803
    public static Bitmap DrawImageWithLabel(string imagePath, string label, float scale, Context context)
    {
        // Text
        var fillPaint = new Android.Graphics.Paint
        {
            AntiAlias = true,
            Color = Color.White,
            TextSize = (context.Resources.DisplayMetrics.Density * TextSize) + 0.5f,
            TextAlign = Android.Graphics.Paint.Align.Left
        };
        var strokePaint = new Android.Graphics.Paint
        {
            AntiAlias = true,
            Color = Color.Black,
            StrokeWidth = StrokeWidth,
            TextSize = (context.Resources.DisplayMetrics.Density * TextSize) + 0.5f,
            TextAlign = Android.Graphics.Paint.Align.Left
        };
        strokePaint.SetStyle(Android.Graphics.Paint.Style.Stroke);

        var baseline = -fillPaint.Ascent();
        var textWidth = (int)(fillPaint.MeasureText(label));
        var textHeight = (int)(baseline + fillPaint.Descent());

        // Image
        var imageFile = new Java.IO.File(context.CacheDir, imagePath);
        var imageBitmap = BitmapFactory.DecodeFile(imageFile.AbsolutePath);

        // Combine
        var finalHeight = textHeight + (int)(imageBitmap.Height * scale) + StrokeWidth;
        var finalWidth = System.Math.Max(textWidth, (int)(imageBitmap.Width * scale)) + StrokeWidth;
        var imageLeft = (finalWidth / 2) - ((int)(imageBitmap.Width * scale) / 2);
        var textLeft = (finalWidth / 2) - (textWidth / 2);
        var finalBitmap = Bitmap.CreateBitmap(finalWidth, finalHeight, Bitmap.Config.Argb8888);
        var canvas = new Canvas(finalBitmap);

        canvas.DrawText(label, textLeft, baseline + StrokeWidth, strokePaint);
        canvas.DrawText(label, textLeft, baseline + StrokeWidth, fillPaint);

        var matrix = new Matrix();
        matrix.PostScale(scale, scale);
        matrix.PostTranslate(imageLeft, (textHeight + StrokeWidth));
        canvas.DrawBitmap(imageBitmap, matrix, null);

        return finalBitmap;
    }

    public static Bitmap DrawImage(string imagePath, float scale, Context context)
    {
        // Image
        var imageFile = new Java.IO.File(context.CacheDir, imagePath);
        var imageBitmap = BitmapFactory.DecodeFile(imageFile.AbsolutePath);

        // Combine
        var finalHeight = (int)(imageBitmap.Height * scale);
        var finalWidth = (int)(imageBitmap.Width * scale);
        var finalBitmap = Bitmap.CreateBitmap(finalWidth, finalHeight, Bitmap.Config.Argb8888);
        var canvas = new Canvas(finalBitmap);

        var matrix = new Matrix();
        matrix.PostScale(scale, scale);
        canvas.DrawBitmap(imageBitmap, matrix, null);

        return finalBitmap;
    }
}
