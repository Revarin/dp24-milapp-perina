using Kris.Client.Data.Cache;
using Kris.Client.Data.Models;

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
    private List<IBackgroundHandler> _handlers = new List<IBackgroundHandler>();

    private ConnectionSettingsEntity _connectionSetting;
    private UserIdentityEntity _userIdentity;
    private uint _iteration = 0;

    public BackgroundLoop(ISettingsStore settingsStore, IIdentityStore identityStore)
    {
        _settingsStore = settingsStore;
        _identityStore = identityStore;
    }

    public void RegisterHandler(IBackgroundHandler handler)
    {
        _handlers.Add(handler);
    }

    public void UnregisterHandler(IBackgroundHandler handler)
    {
        handler.ResetLastUpdate();
        _handlers.Remove(handler);
    }

    public void ClearHandlers()
    {
        _handlers.ForEach(h => h.ResetLastUpdate());
        _handlers.Clear();
    }

    public void ResetHandlers()
    {
        _handlers.ForEach(h => h.ResetLastUpdate());
    }

    public Task StartAsync(CancellationToken ct)
    {
        return Task.Run(async () =>
        {
            _connectionSetting = _settingsStore.GetConnectionSettings();
            _userIdentity = _identityStore.GetIdentity();

            try
            {
                IsRunning = true;

                while (!ct.IsCancellationRequested)
                {
                    if (ReloadSettings)
                    {
                        _connectionSetting = _settingsStore.GetConnectionSettings();
                        _userIdentity = _identityStore.GetIdentity();
                        ReloadSettings = false;
                    }

                    foreach (var handler in _handlers)
                    {
                        await handler.ExecuteAsync(_connectionSetting, _userIdentity, _iteration, ct);
                    }

                    _iteration++;
                    await Task.Delay(_delay, ct);
                }
            }
            finally
            {
                IsRunning = false;
            }
        }, ct);
    }

    public async Task ExecuteAsync(CancellationToken ct)
    {
        if (ReloadSettings || _connectionSetting == null || _userIdentity == null)
        {
            _connectionSetting = _settingsStore.GetConnectionSettings();
            _userIdentity = _identityStore.GetIdentity();
            ReloadSettings = false;
        }

        foreach (var handler in _handlers)
        {
            await handler.ExecuteAsync(_connectionSetting, _userIdentity, _iteration, ct);
        }

        _iteration++;
    }
}
