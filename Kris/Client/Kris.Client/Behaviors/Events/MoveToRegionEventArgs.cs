using Microsoft.Maui.Maps;

namespace Kris.Client.Behaviors.Events;

public sealed class MoveToRegionEventArgs : EventArgs
{
    public MapSpan Region { get; init; }

    public MoveToRegionEventArgs(MapSpan region)
    {
        Region = region;
    }
}
