using Kris.Client.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Graphics.Platform;
using MobileCoreServices;
using UIKit;

namespace Kris.Client.Core.Platforms;

// Source: https://stackoverflow.com/a/77416433
public sealed class ImageService : IImageService
{
    private readonly ILogger<ImageService> _logger;

    public ImageService(ILogger<ImageService> logger)
    {
        _logger = logger;
    }

    public async Task<FileResult> CapturePhotoAsync(MediaPickerOptions options = null)
    {
        return await MainThread.InvokeOnMainThreadAsync(async () =>
            await InternalCapturePhotoAsync(options));
    }

    private async Task<FileResult> InternalCapturePhotoAsync(MediaPickerOptions options)
    {
        var taskCompletionSource = new TaskCompletionSource<FileResult>();
        if (await Permissions.RequestAsync<Permissions.Camera>() == PermissionStatus.Granted)
        {
            var imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.Camera,
                MediaTypes = new string[] { UTType.Image }
            };

            if (!string.IsNullOrEmpty(options?.Title))
            {
                imagePicker.Title = options.Title;
            }

            var viewController = Platform.GetCurrentUIViewController();

            imagePicker.AllowsEditing = false;
            imagePicker.FinishedPickingMedia += async (sender, e) =>
            {
                var jpegFilename = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid()}.jpg");
                var uiImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
                var normalizedImage = uiImage.NormalizeOrientation();
                var normalizedData = normalizedImage.AsJPEG(new nfloat(1));
                await viewController.DismissViewControllerAsync(true);
                if (normalizedData.Save(jpegFilename, false, out var error))
                {
                    taskCompletionSource.TrySetResult(new FileResult(jpegFilename));
                }
                else
                {
                    taskCompletionSource.TrySetException(new Exception($"Error saving the image: {error}"));
                }
                imagePicker?.Dispose();
                imagePicker = null;
            };

            imagePicker.Canceled += async (sender, e) =>
            {
                await viewController.DismissViewControllerAsync(true);
                taskCompletionSource.TrySetResult(null);
                imagePicker?.Dispose();
                imagePicker = null;
            };

            await viewController.PresentViewControllerAsync(imagePicker, true);
        }
        else
        {
            taskCompletionSource.TrySetResult(null);
            taskCompletionSource.TrySetException(new PermissionException("Camera permission not granted"));
        }

        return await taskCompletionSource.Task;
    }
}
