using Kris.Client.Data.Cache;

namespace Kris.Client.Core.Listeners;

public sealed class BackgroundLoop : IBackgroundLoop
{
    private readonly ISettingsStore _settingsStore;
    private readonly IIdentityStore _identityStore;
    private readonly object reloadSettingsLock = new object();

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

    private bool _reloadSettings;
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(1);
    private List<IBackgroundHandler> handlers = new List<IBackgroundHandler>();

    public BackgroundLoop(ISettingsStore settingsStore, IIdentityStore identityStore)
    {
        _settingsStore = settingsStore;
        _identityStore = identityStore;
    }

    public void RegisterHandler(IBackgroundHandler handler)
    {
        handlers.Add(handler);
    }

    public void UnregisterHandler(IBackgroundHandler handler)
    {
        handler.ResetLastUpdate();
        handlers.Remove(handler);
    }

    public void ClearHandlers()
    {
        handlers.ForEach(h => h.ResetLastUpdate());
        handlers.Clear();
    }

    public void ResetHandlers()
    {
        handlers.ForEach(h => h.ResetLastUpdate());
    }

    public Task Start(CancellationToken ct)
    {
        return Task.Run(async () =>
        {
            var connectionSettings = _settingsStore.GetConnectionSettings();
            var userIdentity = _identityStore.GetIdentity();
            uint iter = 0;

            try
            {
                IsRunning = true;

                while (!ct.IsCancellationRequested)
                {
                    if (ReloadSettings)
                    {
                        connectionSettings = _settingsStore.GetConnectionSettings();
                        userIdentity = _identityStore.GetIdentity();
                        ReloadSettings = false;
                    }

                    foreach (var handler in handlers)
                    {
                        await handler.ExecuteAsync(connectionSettings, userIdentity, iter, ct);
                    }

                    iter++;
                    await Task.Delay(_delay, ct);
                }
            }
            finally
            {
                IsRunning = false;
            }
        }, ct);
    }
}
