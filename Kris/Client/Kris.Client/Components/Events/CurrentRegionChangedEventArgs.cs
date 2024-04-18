using Microsoft.Maui.Maps;

namespace Kris.Client.Components.Events;

public sealed class CurrentRegionChangedEventArgs : EventArgs
{
    public MapSpan CurrentRegion { get; init; }

    public CurrentRegionChangedEventArgs(MapSpan currentRegion)
    {
        CurrentRegion = currentRegion;
    }
}
