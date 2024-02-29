using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.User;

public sealed class StoreUserSettingsCommandHandler : UserHandler, IRequestHandler<StoreUserSettingsCommand, Result>
{
    public StoreUserSettingsCommandHandler(IUserRepository userRepository, IUserMapper mapper)
        : base(userRepository, mapper)
    {
    }

    public async Task<Result> Handle(StoreUserSettingsCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithSettingsAsync(request.User.UserId, cancellationToken);
        if (user == null || user.Settings == null) throw new NullableException();
        if (user.Login != request.User.Login) return Result.Fail(new UnauthorizedError("Invalid token"));

        var newSettings = request.StoreUserSettings;
        if (newSettings.ConnectionSettings != null)
        {
            user.Settings.GpsRequestInterval = newSettings.ConnectionSettings.GpsRequestInterval;
            user.Settings.PositionUploadFrequency = newSettings.ConnectionSettings.PositionUploadFrequency;
            user.Settings.PositionDownloadFrequency = newSettings.ConnectionSettings.PositionDownloadFrequency;
            user.Settings.MapObjectDownloadFrequency = newSettings.ConnectionSettings.MapObjectDownloadFrequency;
        }

        await _userRepository.UpdateAsync(cancellationToken);

        return Result.Ok();
    }
}
