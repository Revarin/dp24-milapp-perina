
namespace Kris.Client.Core.Services;

public sealed class ClipboardService : IClipboardService
{
    public async Task ClearAsync()
    {
        await Clipboard.Default.SetTextAsync(null);
    }

    public async Task<string> GetAsync(string text)
    {
        return await Clipboard.Default.GetTextAsync();
    }

    public async Task SetAsync(string text)
    {
        await Clipboard.Default.SetTextAsync(text);
    }
}
