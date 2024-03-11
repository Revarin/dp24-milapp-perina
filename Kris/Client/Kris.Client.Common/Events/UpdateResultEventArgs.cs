using FluentResults;

namespace Kris.Client.Common.Events;

public sealed class UpdateResultEventArgs : ResultEventArgs
{
    public UpdateResultEventArgs(Result result) : base(result)
    {
    }
}

public sealed class UpdateResultEventArgs<T> : ResultEventArgs<T>
{
    public UpdateResultEventArgs(Result<T> result) : base(result)
    {
    }
}
