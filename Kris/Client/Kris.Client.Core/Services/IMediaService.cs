namespace Kris.Client.Core.Services;

public interface IMediaService
{
    Task<FileResult> PickImageAsync();
    Task<FileResult> TakePhotoAsync();
    Task<FileResult> PickMapTileDatabaseAsync();
}
