namespace Kris.Client.Components.Events;

public sealed class MapLongClickedEventArgs : EventArgs
{
    public Location Location { get; set; }

    public MapLongClickedEventArgs(Location location)
    {
        Location = location;
    }
}
