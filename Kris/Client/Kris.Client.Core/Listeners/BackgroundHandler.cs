using FluentResults;
using Kris.Client.Common.Events;
using Kris.Client.Data.Models;

namespace Kris.Client.Core.Listeners;

public abstract class BackgroundHandler : IBackgroundHandler
{
    public event EventHandler<ResultEventArgs> ErrorOccured;

    public abstract Task ExecuteAsync(ConnectionSettingsEntity connectionSettings, UserIdentityEntity userIdentity, uint iteration, CancellationToken ct);

    public abstract void ResetLastUpdate();

    protected void OnErrorOccured(Result result)
    {
        Application.Current.Dispatcher.Dispatch(() => ErrorOccured?.Invoke(this, new ResultEventArgs(result)));
    }
}
