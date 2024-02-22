using FluentResults;

namespace Kris.Client.Events;

public sealed class ResultEventArgs : EventArgs
{
    public Result Result { get; private set; }

    public ResultEventArgs(Result result)
    {
        Result = result;
    }
}

public sealed class ResultEventArgs<T> : EventArgs
{
    public Result<T> Result { get; private set; }

    public ResultEventArgs(Result<T> result)
    {
        Result = result;
    }
}
