using FluentResults;
using Kris.Common.Enums;

namespace Kris.Server.Common.Errors;

public sealed class UnauthorizedError : Error
{
    public UnauthorizedError(string user, string? session, UserType? type)
        : base($"User {user} is not {type} in session {session}")
    {
    }

    public UnauthorizedError(string message)
        : base(message)
    {
    }
}
