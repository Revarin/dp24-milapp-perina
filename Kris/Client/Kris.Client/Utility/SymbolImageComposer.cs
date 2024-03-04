using Kris.Common.Enums;
using SkiaSharp;

namespace Kris.Client.Utility;

public sealed class SymbolImageComposer : ISymbolImageComposer
{
    private const string ImageResourcePath = "Kris.Client.Resources.Images.Point";

    public Stream ComposeMapPointSymbol(MapPointSymbolShape? pointShape, MapPointSymbolColor? pointColor, MapPointSymbolSign? pointSign)
    {
        if (!pointShape.HasValue || pointShape.Value == MapPointSymbolShape.None) return null;

        var shape = pointShape.Value;
        var shapeImageStream = typeof(App).Assembly.GetManifestResourceStream($"{ImageResourcePath}.{PointShapeFileName(shape)}");
        var shapeImage = SKBitmap.Decode(shapeImageStream);

        var resultImage = new SKBitmap(shapeImage.Width, shapeImage.Height);
        using var canvas = new SKCanvas(resultImage);

        DrawAdd(canvas, shapeImage);

        if (pointColor.HasValue && pointColor.Value != MapPointSymbolColor.None)
        {
            var color = pointColor.Value;
            var colorImageStream = typeof(App).Assembly.GetManifestResourceStream($"{ImageResourcePath}.{PointColorFileName(color)}");
            var colorImage = SKBitmap.Decode(colorImageStream);

            DrawAlphaMultiply(canvas, colorImage);
        }

        if (pointSign.HasValue && pointSign.Value != MapPointSymbolSign.None)
        {
            var sign = pointSign.Value;
            var signImageStream = typeof(App).Assembly.GetManifestResourceStream($"{ImageResourcePath}.{PointSignFileName(sign)}");
            var signImage = SKBitmap.Decode(signImageStream);

            DrawAdd(canvas, signImage);
        }

        var resultImageStream = new MemoryStream();
        resultImage.Encode(resultImageStream, SKEncodedImageFormat.Png, 100);
        resultImageStream.Seek(0, SeekOrigin.Begin);

        return resultImageStream;
    }

    private string PointShapeFileName(MapPointSymbolShape shape) => $"point_shape_{shape.ToString().ToLower()}.png";
    private string PointColorFileName(MapPointSymbolColor color) => $"point_color_{color.ToString().ToLower()}.png";
    private string PointSignFileName(MapPointSymbolSign sign) => $"point_sign_{sign.ToString().ToLower()}.png";

    private void DrawAdd(SKCanvas canvas, SKBitmap source)
    {
        canvas.DrawBitmap(source, 0, 0);
    }

    private void DrawAlphaMultiply(SKCanvas canvas, SKBitmap source)
    {
        var paint = new SKPaint { BlendMode = SKBlendMode.Modulate };
        canvas.DrawBitmap(source, 0, 0, paint);
    }
}
