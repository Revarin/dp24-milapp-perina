using FluentResults;

namespace Kris.Client.Common.Errors;

public sealed class UnauthorizedError : Error
{
    public UnauthorizedError()
    {
    }

    public UnauthorizedError(string message) : base(message)
    {
    }
}
