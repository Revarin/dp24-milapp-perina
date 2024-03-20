namespace Kris.Client.Core.Services;

public sealed class MediaService : IMediaService
{
    public async Task<FileResult> PickImageAsync()
    {
        if (!MediaPicker.Default.IsCaptureSupported) return null;
        var result = await MediaPicker.Default.PickPhotoAsync();
        return result;
    }

    public async Task<FileResult> PickMapTileDatabaseAsync()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "application/octet-stream" } },
                { DevicePlatform.iOS, new[] { "" } },
                { DevicePlatform.WinUI, new[] { "" } }
            })
        });
        return result;
    }

    public async Task<FileResult> TakePhotoAsync()
    {
        if (!MediaPicker.Default.IsCaptureSupported) return null;
        var result = await MediaPicker.Default.CapturePhotoAsync();
        return result;
    }
}
