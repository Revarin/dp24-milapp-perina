using Kris.Client.Common.Options;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace Kris.Client.Core.Image;

public sealed class ImageAttachmentComposer : IImageAttachmentComposer
{
    private readonly SettingsOptions _settingsOptions;

    public ImageAttachmentComposer(IOptions<SettingsOptions> options)
    {
        _settingsOptions = options.Value;
    }

    public MemoryStream ComposeScaledImageAttachment(string path, SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Jpeg)
    {
        using var imageFileStream = File.OpenRead(path);
        var image = SKBitmap.Decode(imageFileStream);
        SKBitmap resultImage;

        if (_settingsOptions.ImageAttachmentMaxDimension > 0)
        {
            double finalWidth;
            double finalHeight;

            if (image.Width < image.Height)
            {
                var newWidth = (double)Math.Min(image.Width, _settingsOptions.ImageAttachmentMaxDimension);
                var newHeight = image.Height * (newWidth / image.Width);
                finalHeight = (double)Math.Min(newHeight, _settingsOptions.ImageAttachmentMaxDimension);
                finalWidth = newWidth * (finalHeight / newHeight);
            }
            else
            {
                var newHeight = (double)Math.Min(image.Height, _settingsOptions.ImageAttachmentMaxDimension);
                var newWidth = image.Width * (newHeight / image.Height);
                finalWidth = (double)Math.Min(newWidth, _settingsOptions.ImageAttachmentMaxDimension);
                finalHeight = newHeight * (finalWidth / newWidth);
            }

            resultImage = new SKBitmap((int)finalWidth, (int)finalHeight);
            image.ScalePixels(resultImage, SKFilterQuality.High);
        }
        else
        {
            resultImage = new SKBitmap(image.Width, image.Height);
            image.CopyTo(resultImage);
        }

        var resultImageStream = new MemoryStream();
        resultImage.Encode(resultImageStream, imageFormat, 100);
        resultImageStream.Seek(0, SeekOrigin.Begin);

        return resultImageStream;
    }
}
