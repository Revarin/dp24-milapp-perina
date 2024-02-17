using FluentResults;

namespace Kris.Client.Common.Errors;

public sealed class UserExistsError : Error
{
    public UserExistsError()
    {
    }

    public UserExistsError(string message) : base(message)
    {
    }
}
