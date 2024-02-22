using FluentResults;

namespace Kris.Client.Common.Errors;

public sealed class EntityNotFoundError : Error
{
    public EntityNotFoundError()
    {
    }

    public EntityNotFoundError(string message) : base(message)
    {
    }
}
