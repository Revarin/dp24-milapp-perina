using FluentResults;

namespace Kris.Client.Common.Errors;

public sealed class ServerError : Error
{
    public ServerError()
    {
    }

    public ServerError(string message) : base(message)
    {
    }
}
