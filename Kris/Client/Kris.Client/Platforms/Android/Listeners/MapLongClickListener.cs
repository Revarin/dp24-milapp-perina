using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Kris.Client.Components.Map;

namespace Kris.Client.Platforms.Listeners;

public sealed class MapLongClickListener : Java.Lang.Object, GoogleMap.IOnMapLongClickListener
{
    public KrisMapHandler Handler { get; set; }   

    public void OnMapLongClick(LatLng point)
    {
        Handler.VirtualView.LongClicked(new Location(point.Latitude, point.Longitude));
    }
}
