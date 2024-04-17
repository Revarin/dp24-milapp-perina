using Android.Gms.Maps;
using Kris.Client.Components.Map;

namespace Kris.Client.Platforms.Listeners;

public sealed class MapCameraMoveStartedListener : Java.Lang.Object, GoogleMap.IOnCameraMoveStartedListener
{
    public KrisMapHandler Handler { get; set; }

    public void OnCameraMoveStarted(int reason)
    {
        if (reason == GoogleMap.OnCameraMoveStartedListener.ReasonGesture)
        {
            Handler.VirtualView.MoveStarted();
        }
    }
}
