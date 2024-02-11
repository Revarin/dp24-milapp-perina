using FluentResults;

namespace Kris.Server.Common.Errors;

public sealed class EntityExistsError : Error
{
    public EntityExistsError(string entity, string name)
        : base($"Entity {entity} with name {name} already exists")
    {
    }

    public EntityExistsError(string message)
        : base(message)
    {
    }
}
