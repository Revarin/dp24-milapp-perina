using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Listeners.Events;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;

namespace Kris.Client.Core.Listeners;

public sealed class MapObjectsListener : BackgroundListener, IMapObjectsListener
{
    private readonly IIdentityStore _identityStore;
    private readonly ISettingsStore _settingsStore;
    private readonly IMapObjectController _mapObjectClient;
    private readonly IMapObjectsMapper _mapObjectsMapper;

    public event EventHandler<MapObjectsEventArgs> MapObjectsChanged;

    private DateTime _lastUpdate;

    public MapObjectsListener(IIdentityStore identityStore, ISettingsStore settingsStore,
        IMapObjectController mapObjectClient, IMapObjectsMapper mapObjectsMapper)
    {
        _identityStore = identityStore;
        _settingsStore = settingsStore;
        _mapObjectClient = mapObjectClient;
        _mapObjectsMapper = mapObjectsMapper;
    }

    public override Task StartListening(CancellationToken ct)
    {
        return Task.Run(async () =>
        {
            try
            {
                var settings = _settingsStore.GetConnectionSettings();
                var identity = _identityStore.GetIdentity();

                _lastUpdate = DateTime.MinValue;
                IsListening = true;

                var iter = 0;
                while (!ct.IsCancellationRequested && identity.SessionId.HasValue)
                {
                    var response = await _mapObjectClient.GetMapObjects(_lastUpdate, ct);

                    if (!response.IsSuccess())
                    {
                        if (response.IsUnauthorized()) OnErrorOccured(Result.Fail(new UnauthorizedError()));
                        else OnErrorOccured(Result.Fail(new ServerError(response.Message)));
                    }

                    OnMapObjectsChanged(response.MapPoints.Select(_mapObjectsMapper.Map), response.Resolved);
                    _lastUpdate = response.Resolved;

                    iter++;
                    await Task.Delay(settings.MapObjectDownloadInterval, ct);
                }
            }
            finally
            {
                IsListening = false;
            }
        }, ct);
    }

    private void OnMapObjectsChanged(IEnumerable<MapPointModel> mapPoints, DateTime loaded)
    {
        Application.Current.Dispatcher.Dispatch(() => MapObjectsChanged?.Invoke(this, new MapObjectsEventArgs(mapPoints, loaded)));
    }
}
