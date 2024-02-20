using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Client.Core.Handlers.User;

public sealed class RegisterUserCommandHandler : UserHandler, IRequestHandler<RegisterUserCommand, Result>
{
    public RegisterUserCommandHandler(IUserController userClient, IUserMapper userMapper)
        : base(userClient, userMapper)
    {
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerRequest = new RegisterUserRequest
        {
            Login = request.Login,
            Password = request.Password
        };
        var response = await _userClient.RegisterUser(registerRequest, cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsBadRequest()) return Result.Fail(new EntityExistsError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok();
    }
}
