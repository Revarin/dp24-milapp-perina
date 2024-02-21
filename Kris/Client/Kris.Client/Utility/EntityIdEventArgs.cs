namespace Kris.Client.Utility;

public class EntityIdEventArgs : EventArgs
{
    public Guid Id { get; set; }

    public EntityIdEventArgs(Guid id)
    {
        Id = id;
    }
}
