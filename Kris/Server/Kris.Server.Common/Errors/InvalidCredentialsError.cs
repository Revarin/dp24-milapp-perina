using FluentResults;

namespace Kris.Server.Common.Errors;

public sealed class InvalidCredentialsError : Error
{
    public InvalidCredentialsError()
        : base($"Invalid login credentials")
    {
    }

    public InvalidCredentialsError(string message)
        : base(message)
    {
    }
}
