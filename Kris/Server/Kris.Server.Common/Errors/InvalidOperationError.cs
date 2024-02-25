using FluentResults;

namespace Kris.Server.Common.Errors;

public sealed class InvalidOperationError : Error
{
    public InvalidOperationError(string message) : base(message)
    {
    }
}
