using SkiaSharp;

namespace Kris.Client.Core.Image;

public interface IImageAttachmentComposer
{
    MemoryStream ComposeScaledImageAttachment(string path, SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Jpeg);
}
