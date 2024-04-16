using FluentResults;

namespace Kris.Client.Common.Errors;

public sealed class ConnectionError : Error
{
    public ConnectionError() : base("Connection error")
    {
    }

    public ConnectionError(string message) : base(message)
    {
    }
}
