namespace Kris.Client.Core.Services;

public sealed class GpsService : IGpsService
{
    public async Task<Location> GetCurrentLocationAsync(TimeSpan timeout, CancellationToken ct)
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Best, timeout);
        return await Geolocation.GetLocationAsync(request, ct);
    }

    public async Task<Location> GetLastLocationAsync()
    {
        return await Geolocation.GetLastKnownLocationAsync();
    }

    public Task<bool> IsGpsEnabledAsync()
    {
        throw new NotImplementedException();
    }
}
