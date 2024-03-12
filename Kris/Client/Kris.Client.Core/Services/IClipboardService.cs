namespace Kris.Client.Core.Services;

public interface IClipboardService
{
    Task SetAsync(string text);
    Task<string> GetAsync(string text);
    Task ClearAsync();
}
