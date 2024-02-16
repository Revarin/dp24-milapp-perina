using FluentResults;

namespace Kris.Server.Common.Errors;

public sealed class UserNotInSessionError : Error
{
    public UserNotInSessionError()
        : base($"User is not in session")
    {
    }

    public UserNotInSessionError(string message) : base(message)
    {
    }
}
