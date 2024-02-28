using FluentResults;

namespace Kris.Client.Common.Events;

public class ResultEventArgs : EventArgs
{
    public Result Result { get; init; }

    public ResultEventArgs(Result result)
    {
        Result = result;
    }
}

public class ResultEventArgs<T> : EventArgs
{
    public Result<T> Result { get; init; }

    public ResultEventArgs(Result<T> result)
    {
        Result = result;
    }
}
