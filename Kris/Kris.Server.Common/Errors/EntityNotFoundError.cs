using FluentResults;

namespace Kris.Server.Common.Errors;

public sealed class EntityNotFoundError : Error
{
    public EntityNotFoundError(string entity, Guid id) : base($"Entity {entity} ({id}) not found")
    {
    }

    public EntityNotFoundError(string entity, string name) : base($"Entity {entity} ({name}) not found")
    {
    }
}
