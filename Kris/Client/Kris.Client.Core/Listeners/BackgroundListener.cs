using FluentResults;
using Kris.Client.Common.Events;

namespace Kris.Client.Core.Listeners;

public abstract class BackgroundListener : IBackgroundListener
{
    public event EventHandler<ResultEventArgs> ErrorOccured;
    public bool IsListening { get; protected set; }

    public abstract Task StartListening(CancellationToken ct);

    protected void OnErrorOccured(Result result)
    {
        Application.Current.Dispatcher.Dispatch(() => ErrorOccured?.Invoke(this, new ResultEventArgs(result)));
    }
}
