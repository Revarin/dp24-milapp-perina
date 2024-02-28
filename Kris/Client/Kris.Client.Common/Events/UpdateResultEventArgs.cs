using FluentResults;

namespace Kris.Client.Common.Events;

public sealed class UpdateResultEventArgs : ResultEventArgs
{
    public UpdateResultEventArgs(Result result) : base(result)
    {
    }
}
