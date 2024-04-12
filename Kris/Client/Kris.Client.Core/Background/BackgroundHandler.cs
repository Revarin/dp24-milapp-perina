using FluentResults;
using Kris.Client.Common.Events;
using Kris.Client.Data.Cache;
using Kris.Client.Data.Models;

namespace Kris.Client.Core.Background;

public abstract class BackgroundHandler : IBackgroundHandler
{
    protected readonly ISettingsStore _settingsStore;
    protected readonly IIdentityStore _identityStore;

    public event EventHandler<ResultEventArgs> ErrorOccured;

    public TimeSpan Interval { get; set; }
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

    protected readonly object reloadSettingsLock = new object();
    protected bool _reloadSettings;

    protected ConnectionSettingsEntity _connectionSettings;
    protected UserIdentityEntity _userIdentity;
    protected uint _iteration = 0;

    public BackgroundHandler(ISettingsStore settingsStore, IIdentityStore identityStore)
    {
        _settingsStore = settingsStore;
        _identityStore = identityStore;
        LoadSettings();
    }

    public abstract Task ExecuteAsync(CancellationToken ct);
    protected abstract void LoadSettings();

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
                    await Task.Delay(Interval, ct);
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
