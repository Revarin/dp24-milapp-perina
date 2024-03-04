using Android.Gms.Maps;

namespace Kris.Client.Platforms.Callbacks;

public sealed class OnMapReadyCallback : Java.Lang.Object, IOnMapReadyCallback
{
    private readonly Action<GoogleMap> _handler;

    public OnMapReadyCallback(Action<GoogleMap> handler)
    {
        _handler = handler;
    }

    public void OnMapReady(GoogleMap googleMap)
    {
        _handler?.Invoke(googleMap);
    }
}
