﻿using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Services;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;

namespace Kris.Client.Core.Listeners;

public sealed class CurrentPositionListener : ICurrentPositionListener
{
    private readonly IPermissionService _permissionService;
    private readonly IGpsService _gpsService;
    private readonly IPositionController _positionClient;
    private readonly IPositionMapper _positionMapper;
    private readonly IIdentityStore _identityStore;

    public event EventHandler<LocationEventArgs> PositionChanged;
    public event EventHandler<ResultEventArgs> ErrorOccured;
    public bool IsListening { get; private set; }

    public CurrentPositionListener(IPermissionService permissionService, IGpsService gpsService,
        IPositionController positionClient, IPositionMapper positionMapper, IIdentityStore identityStore)
    {
        _permissionService = permissionService;
        _gpsService = gpsService;
        _positionClient = positionClient;
        _positionMapper = positionMapper;
        _identityStore = identityStore;
    }

    public Task StartListening(CancellationToken ct)
    {
        return Task.Run(async () =>
        {
            try
            {
                // TODO: Settings
                var timeout = TimeSpan.FromSeconds(10);
                var delay = TimeSpan.FromSeconds(5);
                var storage = 3;
                var identity = _identityStore.GetIdentity();

                var locationPermission = await _permissionService.CheckAndRequestPermissionAsync<Permissions.LocationWhenInUse>();
                if (!locationPermission.HasFlag(PermissionStatus.Granted)) return;

                IsListening = true;

                var iter = 0;
                while (!ct.IsCancellationRequested)
                {
                    Location location = null;

                    try
                    {
                        location = await _gpsService.GetCurrentLocationAsync(timeout, ct);
                    }
                    catch (Exception ex)
                    {
                        if (ex is FeatureNotEnabledException) OnErrorOccured(Result.Fail(new ServiceDisabledError()));
                        else if (ex is PermissionException) OnErrorOccured(Result.Fail(new ServicePermissionError()));
                        else throw;
                    }

                    var delayTask = Task.Delay(delay, ct);

                    if (location != null)
                    {
                        OnPositionRead(location, timeout);

                        if (identity.SessionId.HasValue && iter % storage == 0)
                        {
                            await SavePosition(location, ct);
                        }
                    }

                    iter++;
                    await delayTask;
                }
            }
            finally
            {
                IsListening = false;
            }
        }, ct);
    }

    private async Task SavePosition(Location location, CancellationToken ct)
    {
        var httpRequest = new SavePositionRequest
        {
            Position = _positionMapper.Map(location)
        };
        var response = await _positionClient.SavePosition(httpRequest, ct);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized() || response.IsForbidden()) OnErrorOccured(Result.Fail(new UnauthorizedError()));
            else OnErrorOccured(Result.Fail(new ServerError(response.Message)));
        }
    }

    private void OnPositionRead(Location location, TimeSpan difference)
    {
        Application.Current.Dispatcher.Dispatch(() => PositionChanged?.Invoke(this, new LocationEventArgs(location, difference)));
    }

    private void OnErrorOccured(Result result)
    {
        Application.Current.Dispatcher.Dispatch(() => ErrorOccured?.Invoke(this, new ResultEventArgs(result)));
    }
}
