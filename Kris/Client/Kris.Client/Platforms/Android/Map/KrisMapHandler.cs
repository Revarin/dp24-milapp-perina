using Android.Gms.Maps;
using Kris.Client.Platforms.Callbacks;
using Kris.Client.Platforms.Listeners;

namespace Kris.Client.Components.Map;

public partial class KrisMapHandler
{
    private readonly MapLongClickListener _mapLongClickListener = new();

    protected virtual GoogleMap NativeMap { get; private set; }

    protected override async void ConnectHandler(MapView platformView)
    {
        var task = new TaskCompletionSource<GoogleMap>();
        platformView.GetMapAsync(new OnMapReadyCallback(task.SetResult));
        NativeMap = await task.Task;

        var nativeMap = NativeMap;
        if (nativeMap != null)
        {
            _mapLongClickListener.Handler = this;
            nativeMap.SetOnMapLongClickListener(_mapLongClickListener);
        }

        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(MapView platformView)
    {
        var nativeMap = NativeMap;
        if (nativeMap != null)
        {
            nativeMap.SetOnMapLongClickListener(null);
        }

        base.DisconnectHandler(platformView);
    }
}
