namespace Kris.Client.Common.Events;

public sealed class LocationEventArgs : EventArgs
{
    public Guid UserId { get; init; }
    public string UserName { get; init; }
    public Location Location { get; init; }
}
