using FluentResults;

namespace Kris.Client.Common.Errors;

public sealed class ServiceDisabledError : Error
{
    public ServiceDisabledError()
    {
    }

    public ServiceDisabledError(string message) : base(message)
    {
    }
}
