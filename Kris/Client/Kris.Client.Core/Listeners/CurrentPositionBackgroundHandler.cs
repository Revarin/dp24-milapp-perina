using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Listeners.Events;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Core.Services;
using Kris.Client.Data.Models;
using Kris.Common.Enums;
using Kris.Common.Models;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;

namespace Kris.Client.Core.Listeners;

public sealed class CurrentPositionBackgroundHandler : BackgroundHandler, ICurrentPositionBackgroundHandler
{
    private readonly IGpsService _gpsService;
    private readonly IPermissionService _permissionService;
    private readonly IPositionController _positionClient;
    private readonly IPositionMapper _positionMapper;

    public event EventHandler<UserPositionEventArgs> CurrentPositionChanged;

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
            OnLocationRead(new UserPositionModel
            {
                UserId = userIdentity.UserId,
                UserName = userIdentity.CurrentSession?.Nickname ?? userIdentity.Login,
                Positions = new List<Location> { location },
                Symbol = userIdentity.CurrentSession?.Symbol ?? new MapPointSymbol
                {
                    Shape = MapPointSymbolShape.Circle,
                    Color = MapPointSymbolColor.Blue,
                    Sign = MapPointSymbolSign.None
                },
                Updated = DateTime.Now
            });

            // If not in session end here
            if (userIdentity.CurrentSession == null) return;
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

    public override void ResetLastUpdate()
    {
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

    private void OnLocationRead(UserPositionModel userPosition)
    {
        Application.Current.Dispatcher.Dispatch(() => CurrentPositionChanged?.Invoke(this, new UserPositionEventArgs(userPosition)));
    }
}
