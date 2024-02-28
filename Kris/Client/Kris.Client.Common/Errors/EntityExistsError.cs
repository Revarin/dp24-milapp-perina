using FluentResults;

namespace Kris.Client.Common.Errors;

public sealed class EntityExistsError : Error
{
    public EntityExistsError()
    {
    }

    public EntityExistsError(string message) : base(message)
    {
    }
}
