using System;
using Xamarin.Forms.Maps;

namespace Kris.App.Common
{
    public class MoveToRegionRequest
    {
        public event EventHandler<MoveToRegionRequestEventArgs> MoveToRegionRequested;

        public void Execute(MapSpan mapSpan, bool animated = true)
        {
            MoveToRegionRequested?.Invoke(this, new MoveToRegionRequestEventArgs(mapSpan, animated));
        }
    }
}
