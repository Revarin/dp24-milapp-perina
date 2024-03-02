using Kris.Client.Components.Events;

using MauiMap = Microsoft.Maui.Controls.Maps.Map;

namespace Kris.Client.Components.Map;

public sealed class KrisMap : MauiMap, IKrisMap
{
    public event EventHandler<MapLongClickedEventArgs> MapLongClicked;

    public void LongClicked(Location location)
    {
        MapLongClicked?.Invoke(this, new MapLongClickedEventArgs(location));
    }
}
