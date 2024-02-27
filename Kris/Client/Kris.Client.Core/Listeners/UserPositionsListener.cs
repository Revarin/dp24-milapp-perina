using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Listeners.Events;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;

namespace Kris.Client.Core.Listeners;

public sealed class UserPositionsListener : BackgroundListener, IUserPositionsListener
{
    private readonly IPositionController _positionClient;
    private readonly IPositionMapper _positionMapper;
    private readonly ISettingsStore _settingsStore;
    private readonly IIdentityStore _identityStore;

    public event EventHandler<UserPositionsEventArgs> PositionsChanged;

    private DateTime _lastUpdate = DateTime.MinValue;

    public UserPositionsListener(IPositionController positionClient, IPositionMapper positionMapper,
        ISettingsStore settingsStore, IIdentityStore identityStore)
    {
        _positionClient = positionClient;
        _positionMapper = positionMapper;
        _settingsStore = settingsStore;
        _identityStore = identityStore;
    }

    public override Task StartListening(CancellationToken ct)
    {
        return Task.Run(async () =>
        {
            try
            {
                var settings = _settingsStore.GetConnectionSettings();
                var identity = _identityStore.GetIdentity();

                IsListening = true;

                var iter = 0;
                while (!ct.IsCancellationRequested && identity.SessionId.HasValue)
                {
                    var response = await _positionClient.GetPositions(_lastUpdate, ct);

                    if (!response.IsSuccess())
                    {
                        if (response.IsUnauthorized() || response.IsForbidden()) OnErrorOccured(Result.Fail(new UnauthorizedError()));
                        else OnErrorOccured(Result.Fail(new ServerError(response.Message)));
                    }

                    OnUserPositionsChanged(response.UserPositions.Select(_positionMapper.Map), response.Resolved);
                    _lastUpdate = response.Resolved;

                    iter++;
                    await Task.Delay(settings.PositionDownloadInterval, ct);
                }
            }
            finally
            {
                IsListening = false;
            }
        }, ct);
    }

    private void OnUserPositionsChanged(IEnumerable<UserPositionModel> positions, DateTime loaded)
    {
        Application.Current.Dispatcher.Dispatch(() => PositionsChanged?.Invoke(this, new UserPositionsEventArgs(positions, loaded)));
    }
}
