namespace Kris.Client.Common.Events;

public class EntityIdEventArgs : EventArgs
{
    public Guid Id { get; init; }

    public EntityIdEventArgs(Guid id)
    {
        Id = id;
    }
}
