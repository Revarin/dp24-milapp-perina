using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Client.Data.Models;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Client.Core.Handlers.User;

public sealed class LoginUserCommandHandler : UserHandler, IRequestHandler<LoginUserCommand, Result>
{
    private readonly IIdentityStore _identityStore;

    public LoginUserCommandHandler(IIdentityStore identityStore, IUserController userClient, IUserMapper userMapper)
        : base(userClient, userMapper)
    {
        _identityStore = identityStore;
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

        // TODO: Use settings
        _identityStore.StoreIdentity(_userMapper.Map(response));

        return Result.Ok();
    }
}
