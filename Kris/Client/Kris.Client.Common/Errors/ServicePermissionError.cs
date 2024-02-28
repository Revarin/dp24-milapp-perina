using FluentResults;

namespace Kris.Client.Common.Errors;

public sealed class ServicePermissionError : Error
{
    public ServicePermissionError()
    {
    }

    public ServicePermissionError(string message) : base(message)
    {
    }
}
