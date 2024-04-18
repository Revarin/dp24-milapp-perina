using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

namespace Kris.Client.Core.Handlers.User;

public sealed class RegisterUserCommandHandler : UserHandler, IRequestHandler<RegisterUserCommand, Result>
{
    public RegisterUserCommandHandler(IUserController userClient, IUserMapper userMapper)
        : base(userClient, userMapper)
    {
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        using var t = Common.Metrics.SentryMetrics.TimerStart("RequestHandler");
        var registerRequest = new RegisterUserRequest
        {
            Login = request.Login,
            Password = request.Password
        };
        Response response;

        try
        {
            response = await _userClient.RegisterUser(registerRequest, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsBadRequest()) return Result.Fail(new EntityExistsError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok();
    }
}
