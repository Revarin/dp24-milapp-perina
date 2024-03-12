using FluentResults;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using MediatR;

namespace Kris.Client.Core.Handlers.Settings;

public sealed class UpdateMapSettingsCommandHandler : SettingsHandler, IRequestHandler<UpdateMapSettingsCommand, Result>
{
    public UpdateMapSettingsCommandHandler(IUserController userClient, ISettingsStore settingsStore, ISettingsMapper settingsMapper)
        : base(userClient, settingsStore, settingsMapper)
    {
    }

    public Task<Result> Handle(UpdateMapSettingsCommand request, CancellationToken cancellationToken)
    {
        _settingsStore.StoreMapSettings(request.MapSettings);

        // TODO: Send to server

        return Task.FromResult(Result.Ok());
    }
}
