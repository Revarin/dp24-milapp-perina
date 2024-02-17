using FluentResults;

namespace Kris.Server.Extensions;

public static class ErrorListExtensions
{
    public static string? FirstMessage(this List<IError> errors)
    {
        return errors.Select(x => x.Message).FirstOrDefault();
    }
}
