using Microsoft.Maui.Maps;

namespace Kris.Client.Behaviors
{
    public class MoveToRegionRequestEventArgs
    {
        public MapSpan MapSpan { get; }

        public MoveToRegionRequestEventArgs(MapSpan mapSpan)
        {
            MapSpan = mapSpan;
        }
    }
}
