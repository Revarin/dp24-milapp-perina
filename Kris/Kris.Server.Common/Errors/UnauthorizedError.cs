using FluentResults;

namespace Kris.Server.Common.Errors;

public sealed class UnauthorizedError : Error
{
    public UnauthorizedError(string user, string? session, string? role)
        : base($"User {user} is not {role} in session {session}")
    {
    }

    public UnauthorizedError(string message)
        : base(message)
    {
    }
}
