namespace Kris.Client.Core.Services;

public interface IGpsService
{
    Task<Location> GetCurrentLocationAsync(TimeSpan timeout, CancellationToken ct);
    Task<Location> GetLastLocationAsync();
    Task<bool> IsGpsEnabledAsync();
}
