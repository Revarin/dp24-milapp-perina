using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Client.Data.Providers;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

namespace Kris.Client.Core.Handlers.User;

public sealed class LoginUserCommandHandler : UserHandler, IRequestHandler<LoginUserCommand, Result>
{
    private readonly IIdentityStore _identityStore;
    private readonly ISettingsStore _settingsStore;
    private readonly ISettingsMapper _settingsMapper;
    private readonly IConnectionSettingsDataProvider _userSettingsDataProvider;
    private readonly IMapSettingsDataProvider _mapSettingsDataProvider;

    public LoginUserCommandHandler(IIdentityStore identityStore, ISettingsStore settingsStore, ISettingsMapper settingsMapper,
        IConnectionSettingsDataProvider userSettingsDataProvider, IMapSettingsDataProvider mapSettingsDataProvider,
        IUserController userClient, IUserMapper userMapper)
        : base(userClient, userMapper)
    {
        _identityStore = identityStore;
        _settingsStore = settingsStore;
        _settingsMapper = settingsMapper;
        _userSettingsDataProvider = userSettingsDataProvider;
        _mapSettingsDataProvider = mapSettingsDataProvider;
    }

    public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        using var t = Common.Metrics.SentryMetrics.TimerStart("RequestHandler");
        var loginRequest = new LoginUserRequest
        {
            Login = request.Login,
            Password = request.Password
        };
        LoginResponse response;

        try
        {
            response = await _userClient.LoginUser(loginRequest, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else return Result.Fail(new ServerError(response.Message));
        }

        _identityStore.StoreIdentity(_userMapper.Map(response));
        if (response.Settings.ConnectionSettings != null)
        {
            _settingsStore.StoreConnectionSettings(_settingsMapper.Map(response.Settings.ConnectionSettings));
        }
        else
        {
            _settingsStore.StoreConnectionSettings(_userSettingsDataProvider.GetDefault());
        }
        if (response.Settings.MapSettings != null)
        {
            _settingsStore.StoreMapSettings(_settingsMapper.Map(response.Settings.MapSettings));
        }
        else
        {
            _settingsStore.StoreMapSettings(_mapSettingsDataProvider.GetDefault());
        }

        return Result.Ok();
    }
}
