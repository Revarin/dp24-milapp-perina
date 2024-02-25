using FluentResults;

namespace Kris.Client.Common.Errors;

public sealed class ForbiddenError : Error
{
    public ForbiddenError()
    {
    }

    public ForbiddenError(string message) : base(message)
    {
    }
}
