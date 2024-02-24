using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Common.Events;
using Kris.Client.Core.Services;
using Kris.Interface.Controllers;

namespace Kris.Client.Core.Listeners;

public sealed class CurrentPositionListener : ICurrentPositionListener
{
    private readonly IPermissionService _permissionService;
    private readonly IGpsService _gpsService;
    private readonly IPositionController _positionClient;

    public event EventHandler<LocationEventArgs> PositionChanged;
    public event EventHandler<ResultEventArgs> ErrorOccured;
    public bool IsListening { get; private set; }

    public CurrentPositionListener(IPermissionService permissionService, IGpsService gpsService, IPositionController positionClient)
    {
        _permissionService = permissionService;
        _gpsService = gpsService;
        _positionClient = positionClient;
    }

    public void StartListening(CancellationToken ct)
    {
        Task.Run(async () =>
        {
            // TODO: Settings
            var timeout = TimeSpan.FromSeconds(10);
            var delay = TimeSpan.FromSeconds(5);

            var locationPermission = await _permissionService.CheckAndRequestPermissionAsync<Permissions.LocationWhenInUse>();
            if (!locationPermission.HasFlag(PermissionStatus.Granted)) return;

            IsListening = true;
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    Location location = null;

                    try
                    {
                        location = await _gpsService.GetCurrentLocationAsync(timeout, ct);
                    }
                    catch (Exception ex)
                    {
                        if (ex is FeatureNotEnabledException) OnErrorOccured(new ResultEventArgs(Result.Fail(new ServiceDisabledError())));
                        else if (ex is PermissionException) OnErrorOccured(new ResultEventArgs(Result.Fail(new ServicePermissionError())));
                        else throw;
                    }

                    if (location != null)
                    {
                        OnPositionRead(new LocationEventArgs(location, timeout));
                        // TODO: SEND TO SERVER
                    }

                    await Task.Delay(delay, ct);
                }
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                IsListening = false;
            }
        });
    }

    private void OnPositionRead(LocationEventArgs e)
    {
        Application.Current.Dispatcher.Dispatch(() => PositionChanged?.Invoke(this, e));
    }

    private void OnErrorOccured(ResultEventArgs e)
    {
        Application.Current.Dispatcher.Dispatch(() => ErrorOccured?.Invoke(this, e));
    }
}
