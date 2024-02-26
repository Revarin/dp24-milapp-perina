using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Client.Core.Handlers.User;

public sealed class LoginUserCommandHandler : UserHandler, IRequestHandler<LoginUserCommand, Result>
{
    private readonly IIdentityStore _identityStore;
    private readonly ISettingsStore _settingsStore;
    private readonly ISettingsMapper _settingsMapper;

    public LoginUserCommandHandler(IIdentityStore identityStore, ISettingsStore settingsStore, ISettingsMapper settingsMapper,
        IUserController userClient, IUserMapper userMapper)
        : base(userClient, userMapper)
    {
        _identityStore = identityStore;
        _settingsStore = settingsStore;
        _settingsMapper = settingsMapper;
    }

    public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var loginRequest = new LoginUserRequest
        {
            Login = request.Login,
            Password = request.Password
        };
        var response = await _userClient.LoginUser(loginRequest, cancellationToken);

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

        return Result.Ok();
    }
}
