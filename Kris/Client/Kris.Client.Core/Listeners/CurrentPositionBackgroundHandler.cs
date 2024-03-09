using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Listeners.Events;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Services;
using Kris.Client.Data.Models;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;

namespace Kris.Client.Core.Listeners;

public sealed class CurrentPositionBackgroundHandler : BackgroundHandler, ICurrentPositionBackgroundHandler
{
    private readonly IGpsService _gpsService;
    private readonly IPermissionService _permissionService;
    private readonly IPositionController _positionClient;
    private readonly IPositionMapper _positionMapper;

    public event EventHandler<LocationEventArgs> CurrentPositionChanged;

    private PermissionStatus _permissionStatus = PermissionStatus.Unknown;

    public CurrentPositionBackgroundHandler(IGpsService gpsService, IPermissionService permissionService,
        IPositionController positionClient, IPositionMapper positionMapper)
    {
        _gpsService = gpsService;
        _permissionService = permissionService;
        _positionClient = positionClient;
        _positionMapper = positionMapper;
    }

    public override async Task ExecuteAsync(ConnectionSettingsEntity connectionSettings, UserIdentityEntity userIdentity, uint iteration, CancellationToken ct)
    {
        if (iteration % (uint)connectionSettings.GpsInterval.TotalSeconds != 0) return;
        if (_permissionStatus == PermissionStatus.Denied || _permissionStatus == PermissionStatus.Disabled) return;
        if (_permissionStatus == PermissionStatus.Unknown) await AskForGpsPermissionAsync();

        Location location = null;

        try
        {
            location = await _gpsService.GetCurrentLocationAsync(connectionSettings.GpsInterval.Multiply(2), ct);
        }
        catch (Exception ex)
        {
            if (ex is FeatureNotEnabledException) OnErrorOccured(Result.Fail(new ServiceDisabledError(nameof(Permissions.LocationWhenInUse))));
            else if (ex is PermissionException) OnErrorOccured(Result.Fail(new ServicePermissionError(nameof(Permissions.LocationWhenInUse))));
            else throw;
        }

        if (location != null)
        {
            OnLocationRead(userIdentity.UserId, userIdentity.Login, location);

            if (!userIdentity.SessionId.HasValue) return;
            if (iteration % (uint)connectionSettings.GpsInterval.TotalSeconds != 0) return;

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
    }

    private async Task AskForGpsPermissionAsync()
    {
        _permissionStatus = await _permissionService.CheckPermissionAsync<Permissions.LocationWhenInUse>();
        if (_permissionStatus != PermissionStatus.Granted)
        {
            _permissionStatus = await _permissionService.CheckAndRequestPermissionAsync<Permissions.LocationWhenInUse>();
            if (_permissionStatus != PermissionStatus.Granted) return;
        }
    }

    private void OnLocationRead(Guid userId, string userName, Location location)
    {
        Application.Current.Dispatcher.Dispatch(() => CurrentPositionChanged?.Invoke(this, new LocationEventArgs
        {
            UserId = userId,
            UserName = userName,
            Location = location
        }));
    }
}
