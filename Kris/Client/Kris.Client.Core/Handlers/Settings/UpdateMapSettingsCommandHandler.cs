using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Client.Core.Handlers.Settings;

public sealed class UpdateMapSettingsCommandHandler : SettingsHandler, IRequestHandler<UpdateMapSettingsCommand, Result>
{
    public UpdateMapSettingsCommandHandler(IUserController userClient, ISettingsStore settingsStore, ISettingsMapper settingsMapper)
        : base(userClient, settingsStore, settingsMapper)
    {
    }

    public async Task<Result> Handle(UpdateMapSettingsCommand request, CancellationToken cancellationToken)
    {
        _settingsStore.StoreMapSettings(request.MapSettings);
        if (request.CustomMapTilesDatabasePath != null)
        {
            _settingsStore.StoreMapTilesSourcePath(request.CustomMapTilesDatabasePath);
        }

        var httpRequest = new StoreUserSettingsRequest
        {
            MapSettings = _settingsMapper.Map(request.MapSettings)
        };
        var response = await _userClient.StoreUserSettings(httpRequest, cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok();
    }
}
