using FluentResults;

namespace Kris.Client.Common.Errors;

public sealed class BadOperationError : Error
{
    public BadOperationError()
    {
    }

    public BadOperationError(string message) : base(message)
    {
    }
}
