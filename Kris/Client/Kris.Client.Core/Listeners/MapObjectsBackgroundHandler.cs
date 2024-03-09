using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Listeners.Events;
using Kris.Client.Core.Mappers;
using Kris.Client.Data.Models;
using Kris.Client.Core.Models;
using Kris.Interface.Controllers;

namespace Kris.Client.Core.Listeners;

public sealed class MapObjectsBackgroundHandler : BackgroundHandler, IMapObjectsBackgroundHandler
{
    private readonly IMapObjectController _mapObjectClient;
    private readonly IMapObjectsMapper _mapObjectsMapper;

    public event EventHandler<MapObjectsEventArgs> MapObjectsChanged;

    private DateTime _lastUpdate = DateTime.MinValue;

    public MapObjectsBackgroundHandler(IMapObjectController mapObjectClient, IMapObjectsMapper mapObjectsMapper)
    {
        _mapObjectClient = mapObjectClient;
        _mapObjectsMapper = mapObjectsMapper;
    }

    public override async Task ExecuteAsync(ConnectionSettingsEntity connectionSettings, UserIdentityEntity userIdentity, uint iteration, CancellationToken ct)
    {
        if (iteration % (uint)connectionSettings.PositionDownloadInterval.TotalSeconds != 0) return;
        if (!userIdentity.SessionId.HasValue) return;

        var response = await _mapObjectClient.GetMapObjects(_lastUpdate, ct);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) OnErrorOccured(Result.Fail(new UnauthorizedError()));
            else OnErrorOccured(Result.Fail(new ServerError(response.Message)));
        }

        OnMapObjectsChanged(response.MapPoints.Select(_mapObjectsMapper.Map), response.Resolved);
        _lastUpdate = response.Resolved;

    }

    private void OnMapObjectsChanged(IEnumerable<MapPointModel> mapPoints, DateTime loaded)
    {
        Application.Current.Dispatcher.Dispatch(() => MapObjectsChanged?.Invoke(this, new MapObjectsEventArgs(mapPoints, loaded)));
    }
}
