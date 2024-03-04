using Kris.Client.Components.Events;
using Microsoft.Maui.Maps;

namespace Kris.Client.Utility;

public sealed class MoveToRegionRequest
{
    public event EventHandler<MoveToRegionEventArgs> MoveToRegionRequested;

    public void Execute(MapSpan mapSpan)
    {
        MoveToRegionRequested?.Invoke(this, new MoveToRegionEventArgs(mapSpan));
    }
}
