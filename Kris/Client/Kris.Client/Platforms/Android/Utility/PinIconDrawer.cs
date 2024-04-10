using Android.Content;
using Android.Graphics;

using Color = Android.Graphics.Color;

namespace Kris.Client.Platforms.Utility;

public static class PinIconDrawer
{
    private const int StrokeWidth = 8;
    private const float Scale = 1.5f;
    private const float TextSize = 14f;

    // Source: https://stackoverflow.com/a/40192803
    public static Bitmap DrawImageWithLabel(string imageName, string label, bool isLight, Context context)
    {
        // Text
        var fillPaint = new Android.Graphics.Paint
        {
            AntiAlias = true,
            Color = isLight ? Color.Black : Color.White,
            TextSize = (context.Resources.DisplayMetrics.Density * TextSize) + 0.5f,
            TextAlign = Android.Graphics.Paint.Align.Left
        };
        var strokePaint = new Android.Graphics.Paint
        {
            AntiAlias = true,
            Color = isLight ? Color.White : Color.Black,
            StrokeWidth = StrokeWidth,
            TextSize = (context.Resources.DisplayMetrics.Density * TextSize) + 0.5f,
            TextAlign = Android.Graphics.Paint.Align.Left
        };
        strokePaint.SetStyle(Android.Graphics.Paint.Style.Stroke);

        var baseline = -fillPaint.Ascent();
        var textWidth = (int)(fillPaint.MeasureText(label));
        var textHeight = (int)(baseline + fillPaint.Descent());

        // Image
        var imageFile = new Java.IO.File(context.CacheDir, imageName);
        var imageBitmap = BitmapFactory.DecodeFile(imageFile.AbsolutePath);

        // Combine
        var finalHeight = textHeight + (int)(imageBitmap.Height * Scale) + StrokeWidth;
        var finalWidth = System.Math.Max(textWidth, (int)(imageBitmap.Width * Scale)) + StrokeWidth;
        var imageLeft = (finalWidth / 2) - ((int)(imageBitmap.Width * Scale) / 2);
        var finalBitmap = Bitmap.CreateBitmap(finalWidth, finalHeight, Bitmap.Config.Argb8888);
        var canvas = new Canvas(finalBitmap);

        canvas.DrawText(label, 0, baseline + StrokeWidth, strokePaint);
        canvas.DrawText(label, 0, baseline + StrokeWidth, fillPaint);

        var matrix = new Matrix();
        matrix.PostScale(Scale, Scale);
        matrix.PostTranslate(imageLeft, (textHeight + StrokeWidth));
        canvas.DrawBitmap(imageBitmap, matrix, null);

        return finalBitmap;
    }
}
