using FluentResults;
using Kris.Client.Common.Events;
using Kris.Client.Data.Cache;

namespace Kris.Client.Core.Background;

public abstract class BackgroundHandler : IBackgroundHandler
{
    protected readonly ISettingsStore _settingsStore;
    protected readonly IIdentityStore _identityStore;

    public event EventHandler<ResultEventArgs> ErrorOccured;

    public bool IsRunning { get; private set; }
    public bool ReloadSettings
    {
        get
        {
            lock (reloadSettingsLock)
            {
                return _reloadSettings;
            }
        }
        set
        {
            lock (reloadSettingsLock)
            {
                _reloadSettings = value;
            }
        }
    }
    public TimeSpan Interval { get; set; }

    protected readonly object reloadSettingsLock = new object();
    protected bool _reloadSettings;

    public BackgroundHandler(ISettingsStore settingsStore, IIdentityStore identityStore)
    {
        _settingsStore = settingsStore;
        _identityStore = identityStore;
    }

    public abstract Task ExecuteAsync(CancellationToken ct);

    public Task StartLoopAsync(CancellationToken ct)
    {
        return Task.Run(async () =>
        {
            try
            {
                IsRunning = true;

                while (!ct.IsCancellationRequested)
                {
                    await ExecuteAsync(ct);
                }
            }
            finally
            {
                IsRunning = false;
            }
        }, ct);
    }

    protected void OnErrorOccured(Result result)
    {
        Application.Current.Dispatcher.Dispatch(() => ErrorOccured?.Invoke(this, new ResultEventArgs(result)));
    }
}
