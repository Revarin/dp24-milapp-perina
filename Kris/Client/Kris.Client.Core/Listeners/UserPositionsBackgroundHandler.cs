using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Listeners.Events;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Data.Models;
using Kris.Interface.Controllers;

namespace Kris.Client.Core.Listeners;

public sealed class UserPositionsBackgroundHandler : BackgroundHandler, IUserPositionsBackgroundHandler
{
    private readonly IPositionController _positionClient;
    private readonly IPositionMapper _positionMapper;

    public event EventHandler<UserPositionsEventArgs> UserPositionsChanged;

    private DateTime _lastUpdate = DateTime.MinValue;

    public UserPositionsBackgroundHandler(IPositionController positionClient, IPositionMapper positionMapper)
    {
        _positionClient = positionClient;
        _positionMapper = positionMapper;
    }

    public override async Task ExecuteAsync(ConnectionSettingsEntity connectionSettings, UserIdentityEntity userIdentity, uint iteration, CancellationToken ct)
    {
        if (iteration % (uint)connectionSettings.PositionDownloadInterval.TotalSeconds != 0) return;
        if (!userIdentity.SessionId.HasValue) return;

        var response = await _positionClient.GetPositions(_lastUpdate, ct);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized() || response.IsForbidden()) OnErrorOccured(Result.Fail(new UnauthorizedError()));
            else OnErrorOccured(Result.Fail(new ServerError(response.Message)));
        }

        OnUserPositionsChanged(response.UserPositions.Select(_positionMapper.Map), response.Resolved);
        _lastUpdate = response.Resolved;
    }
    public override void ResetLastUpdate()
    {
        _lastUpdate = DateTime.MinValue;
    }

    private void OnUserPositionsChanged(IEnumerable<UserPositionModel> positions, DateTime loaded)
    {
        Application.Current.Dispatcher.Dispatch(() => UserPositionsChanged?.Invoke(this, new UserPositionsEventArgs(positions, loaded)));
    }
}
