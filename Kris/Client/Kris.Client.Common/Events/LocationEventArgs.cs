namespace Kris.Client.Common.Events;

public sealed class LocationEventArgs : EventArgs
{
    public Location Location { get; init; }
    public TimeSpan Difference { get; init; }

    public LocationEventArgs(Location location, TimeSpan difference)
    {
        Location = location;
        Difference = difference;
    }
}
