using Kris.Client.Core.Services;

namespace Kris.Client.Core.Platforms;

public sealed class ImageService : IImageService
{
    public Task<FileResult> CapturePhotoAsync(MediaPickerOptions options = null)
    {
        throw new NotImplementedException();
    }
}
