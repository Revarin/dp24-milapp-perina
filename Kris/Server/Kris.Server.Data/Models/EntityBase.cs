namespace Kris.Server.Data.Models;

public abstract class EntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
