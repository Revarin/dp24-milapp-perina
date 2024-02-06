using FluentResults;

namespace Kris.Server.Common.Errors;

public sealed class UserExistsError : Error
{
    public UserExistsError(string login) : base($"User {login} already exists")
    {
    }
}
