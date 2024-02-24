using FluentResults;

namespace Kris.Client.Common.Events;

public sealed class ResultEventArgs : EventArgs
{
    public Result Result { get; init; }

    public ResultEventArgs(Result result)
    {
        Result = result;
    }
}

public sealed class ResultEventArgs<T> : EventArgs
{
    public Result<T> Result { get; init; }

    public ResultEventArgs(Result<T> result)
    {
        Result = result;
    }
}
