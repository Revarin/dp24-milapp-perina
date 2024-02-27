using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

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
        var httpRequest = new EditUserRequest
        {
            NewLogin = request.NewLogin,
            NewPassword = request.NewPassword,
            Password = request.Password
        };
        var response = await _userClient.EditUser(httpRequest, cancellationToken);

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
