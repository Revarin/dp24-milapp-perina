using System;
using Xamarin.Forms.Maps;

namespace Kris.App.Common
{
    public class MoveToRegionRequestEventArgs : EventArgs
    {
        public MapSpan MapSpan { get; }
        public bool Animated { get; }

        public MoveToRegionRequestEventArgs(MapSpan mapSpan, bool animated)
        {
            MapSpan = mapSpan;
            Animated = animated;
        }
    }
}
