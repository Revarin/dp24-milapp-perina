namespace Kris.Client.Core.Services;

// Source: https://stackoverflow.com/a/77416433
public interface IImageService
{
    Task<FileResult> CapturePhotoAsync(MediaPickerOptions options = null);
}
