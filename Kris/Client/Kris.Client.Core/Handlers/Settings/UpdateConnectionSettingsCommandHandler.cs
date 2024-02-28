using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Client.Core.Handlers.Settings;

public sealed class UpdateConnectionSettingsCommandHandler : SettingsHandler, IRequestHandler<UpdateConnectionSettingsCommand, Result>
{
    public UpdateConnectionSettingsCommandHandler(IUserController userClient, ISettingsStore settingsStore, ISettingsMapper settingsMapper)
        : base(userClient, settingsStore, settingsMapper)
    {
    }

    public async Task<Result> Handle(UpdateConnectionSettingsCommand request, CancellationToken cancellationToken)
    {
        _settingsStore.StoreConnectionSettings(request.ConnectionSettings);

        var httpRequest = new StoreUserSettingsRequest
        {
            ConnectionSettings = _settingsMapper.Map(request.ConnectionSettings)
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
