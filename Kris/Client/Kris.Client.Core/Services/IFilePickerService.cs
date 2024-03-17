namespace Kris.Client.Core.Services;

public interface IFilePickerService
{
    Task<FileResult> PickImageAsync();
}
