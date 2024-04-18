using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

namespace Kris.Client.Core.Handlers.User;

public sealed class EditUserCommandHandler : UserHandler, IRequestHandler<EditUserCommand, Result>
{
    private readonly IIdentityStore _identityStore;

    public EditUserCommandHandler(IIdentityStore identityStore,
        IUserController userClient, IUserMapper userMapper)
        : base(userClient, userMapper)
    {
        _identityStore = identityStore;
    }

    public async Task<Result> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        using var t = Common.Metrics.SentryMetrics.TimerStart("RequestHandler");
        var httpRequest = new EditUserRequest
        {
            NewPassword = request.NewPassword,
            Password = request.Password
        };
        IdentityResponse response;

        try
        {
            response = await _userClient.EditUser(httpRequest, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsForbidden()) return Result.Fail(new ForbiddenError());
            else return Result.Fail(new ServerError(response.Message));
        }

        _identityStore.StoreIdentity(_userMapper.Map(response));

        return Result.Ok();
    }
}
