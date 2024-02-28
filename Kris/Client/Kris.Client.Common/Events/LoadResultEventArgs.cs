using FluentResults;

namespace Kris.Client.Common.Events;

public sealed class LoadResultEventArgs<T> : ResultEventArgs<T>
{
    public LoadResultEventArgs(Result<T> result) : base(result)
    {
    }
}
