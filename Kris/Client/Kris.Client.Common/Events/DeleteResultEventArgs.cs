using FluentResults;

namespace Kris.Client.Common.Events;

public sealed class DeleteResultEventArgs : ResultEventArgs
{
    public DeleteResultEventArgs(Result result) : base(result)
    {
    }
}
