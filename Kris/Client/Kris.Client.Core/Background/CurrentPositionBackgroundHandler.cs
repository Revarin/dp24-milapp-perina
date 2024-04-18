using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Background.Events;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Core.Services;
using Kris.Client.Data.Cache;
using Kris.Common.Enums;
using Kris.Common.Models;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using System.Net;

namespace Kris.Client.Core.Background;

public sealed class CurrentPositionBackgroundHandler : BackgroundHandler, ICurrentPositionBackgroundHandler
{
    private readonly IGpsService _gpsService;
    private readonly IPermissionService _permissionService;
    private readonly IPositionController _positionClient;
    private readonly IPositionMapper _positionMapper;

    public event EventHandler<UserPositionEventArgs> CurrentPositionChanged;

    public new bool IsRunning { get; set; }

    private PermissionStatus _permissionStatus = PermissionStatus.Unknown;

    public CurrentPositionBackgroundHandler(IGpsService gpsService, IPermissionService permissionService,
        IPositionController positionClient, IPositionMapper positionMapper,
        ISettingsStore settingsStore, IIdentityStore identityStore)
        : base(settingsStore, identityStore)
    {
        _gpsService = gpsService;
        _permissionService = permissionService;
        _positionClient = positionClient;
        _positionMapper = positionMapper;
    }

    public override async Task ExecuteAsync(CancellationToken ct)
    {
        if (ReloadSettings) LoadSettings();
        if (_permissionStatus == PermissionStatus.Denied || _permissionStatus == PermissionStatus.Disabled) return;
        if (_permissionStatus == PermissionStatus.Unknown) await AskForGpsPermissionAsync();

        using var t = Common.Metrics.SentryMetrics.TimerStart("CurrentPositionBackgroundHandler");
        Location location = null;

        try
        {
            location = await _gpsService.GetCurrentLocationAsync(_connectionSettings.GpsInterval.Multiply(2), ct);
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
                UserId = _userIdentity.UserId,
                UserName = _userIdentity.CurrentSession?.Nickname ?? _userIdentity.Login,
                Positions = new List<Location> { location },
                Symbol = _userIdentity.CurrentSession?.Symbol ?? new MapPointSymbol
                {
                    Shape = MapPointSymbolShape.Circle,
                    Color = MapPointSymbolColor.Blue,
                    Sign = MapPointSymbolSign.None
                },
                Updated = DateTime.Now
            });
            _iteration++;

            // If not in session end here
            if (_userIdentity.CurrentSession == null) return;
            if (_iteration % (uint)_connectionSettings.GpsInterval.TotalSeconds != 0) return;

            var httpRequest = new SavePositionRequest
            {
                Position = _positionMapper.Map(location)
            };
            Response response;

            try
            {
                response = await _positionClient.SavePosition(httpRequest, ct);
            }
            catch (WebException)
            {
                return;
            }

            if (!response.IsSuccess())
            {
                if (response.IsUnauthorized() || response.IsForbidden()) OnErrorOccured(Result.Fail(new UnauthorizedError()));
                else OnErrorOccured(Result.Fail(new ServerError(response.Message)));
            }
        }
    }

    protected override void LoadSettings()
    {
        _connectionSettings = _settingsStore.GetConnectionSettings();
        _userIdentity = _identityStore.GetIdentity();
        Interval = _connectionSettings.GpsInterval;
        ReloadSettings = false;
    }

    private async Task AskForGpsPermissionAsync()
    {
        _permissionStatus = await _permissionService.CheckPermissionAsync<Permissions.LocationAlways>();
        if (_permissionStatus != PermissionStatus.Granted)
        {
            _permissionStatus = await _permissionService.CheckAndRequestPermissionAsync<Permissions.LocationAlways>();
            if (_permissionStatus != PermissionStatus.Granted) return;
        }
    }

    private void OnLocationRead(UserPositionModel userPosition)
    {
        Application.Current.Dispatcher.Dispatch(() => CurrentPositionChanged?.Invoke(this, new UserPositionEventArgs(userPosition)));
    }
}
