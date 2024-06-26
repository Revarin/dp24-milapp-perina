﻿using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Background.Events;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Responses;
using System.Net;

namespace Kris.Client.Core.Background;

public sealed class MapObjectsBackgroundHandler : BackgroundHandler, IMapObjectsBackgroundHandler
{
    private readonly IMapObjectController _mapObjectClient;
    private readonly IMapObjectsMapper _mapObjectsMapper;

    public event EventHandler<MapObjectsEventArgs> MapObjectsChanged;

    private DateTime _lastUpdate = DateTime.MinValue;

    public MapObjectsBackgroundHandler(IMapObjectController mapObjectClient, IMapObjectsMapper mapObjectsMapper,
        ISettingsStore settingsStore, IIdentityStore identityStore)
        : base(settingsStore, identityStore)
    {
        _mapObjectClient = mapObjectClient;
        _mapObjectsMapper = mapObjectsMapper;
    }

    public override async Task ExecuteAsync(CancellationToken ct)
    {
        if (ReloadSettings) LoadSettings();
        if (_userIdentity.CurrentSession == null) return;

        using var t = Common.Metrics.SentryMetrics.TimerStart("MapObjectsBackgroundHandler");
        GetMapObjectsResponse response;

        try
        {
            response = await _mapObjectClient.GetMapObjects(_lastUpdate, ct);
        }
        catch (WebException)
        {
            return;
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) OnErrorOccured(Result.Fail(new UnauthorizedError()));
            else OnErrorOccured(Result.Fail(new ServerError(response.Message)));
        }

        OnMapObjectsChanged(response.MapPoints.Select(_mapObjectsMapper.MapPoint), response.DeletedMapPoints, response.Resolved);
        _lastUpdate = response.Resolved;
    }

    protected override void LoadSettings()
    {
        _connectionSettings = _settingsStore.GetConnectionSettings();
        _userIdentity = _identityStore.GetIdentity();
        Interval = _connectionSettings.MapObjectDownloadInterval;
        ReloadSettings = false;
        _lastUpdate = DateTime.MinValue;
    }

    private void OnMapObjectsChanged(IEnumerable<MapPointListModel> mapPoints, IEnumerable<Guid> deletedMapPoints, DateTime loaded)
    {
        Application.Current.Dispatcher.Dispatch(() => MapObjectsChanged?.Invoke(this, new MapObjectsEventArgs(mapPoints, deletedMapPoints, loaded)));
    }
}
