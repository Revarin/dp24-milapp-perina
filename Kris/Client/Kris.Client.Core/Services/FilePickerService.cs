namespace Kris.Client.Core.Services;

public sealed class FilePickerService : IFilePickerService
{
    public async Task<FileResult> PickImageAsync()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions { FileTypes = FilePickerFileType.Images });
        return result;
    }
}
