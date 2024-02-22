namespace Kris.Client.Events;

public class EntityIdEventArgs : EventArgs
{
    public Guid Id { get; private set; }

    public EntityIdEventArgs(Guid id)
    {
        Id = id;
    }
}
