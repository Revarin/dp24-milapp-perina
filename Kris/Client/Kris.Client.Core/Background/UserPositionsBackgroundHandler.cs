using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Background.Events;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Responses;
using System.Net;

namespace Kris.Client.Core.Background;

public sealed class UserPositionsBackgroundHandler : BackgroundHandler, IUserPositionsBackgroundHandler
{
    private readonly IPositionController _positionClient;
    private readonly IPositionMapper _positionMapper;

    public event EventHandler<UserPositionsEventArgs> UserPositionsChanged;

    private DateTime _lastUpdate = DateTime.MinValue;

    public UserPositionsBackgroundHandler(IPositionController positionClient, IPositionMapper positionMapper,
        ISettingsStore settingsStore, IIdentityStore identityStore)
        : base(settingsStore, identityStore)
    {
        _positionClient = positionClient;
        _positionMapper = positionMapper;
    }


    public override async Task ExecuteAsync(CancellationToken ct)
    {
        if (ReloadSettings) LoadSettings();
        if (_userIdentity.CurrentSession == null) return;

        GetPositionsResponse response;

        try
        {
            response = await _positionClient.GetPositions(_lastUpdate, ct);
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

        OnUserPositionsChanged(response.UserPositions.Select(_positionMapper.Map), response.Resolved);
        _lastUpdate = response.Resolved;
    }

    protected override void LoadSettings()
    {
        _connectionSettings = _settingsStore.GetConnectionSettings();
        _userIdentity = _identityStore.GetIdentity();
        Interval = _connectionSettings.PositionDownloadInterval;
        ReloadSettings = false;
        _lastUpdate = DateTime.MinValue;
    }

    private void OnUserPositionsChanged(IEnumerable<UserPositionModel> positions, DateTime loaded)
    {
        Application.Current.Dispatcher.Dispatch(() => UserPositionsChanged?.Invoke(this, new UserPositionsEventArgs(positions, loaded)));
    }
}
