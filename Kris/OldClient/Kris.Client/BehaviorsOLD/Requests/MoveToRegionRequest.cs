using Microsoft.Maui.Maps;

namespace Kris.Client.Behaviors
{
    public class MoveToRegionRequest
    {
        public event EventHandler<MoveToRegionRequestEventArgs> MoveToRegionRequested;

        public void Execute(MapSpan mapSpan)
        {
            MoveToRegionRequested?.Invoke(this, new MoveToRegionRequestEventArgs(mapSpan));
        }
    }
}
