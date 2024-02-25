using FluentResults;

namespace Kris.Common.Extensions;

public static class ErrorListExtensions
{
    public static string? FirstMessage(this List<IError> errors)
    {
        return errors.Select(x => x.Message).FirstOrDefault();
    }
}
