using FluentResults;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using MediatR;

namespace Kris.Client.Core.Handlers.User;

public sealed class GetCurrentUserQueryHandler : UserHandler, IRequestHandler<GetCurrentUserQuery, CurrentUserModel>
{
    private readonly IIdentityStore _identityStore;

    public GetCurrentUserQueryHandler(IIdentityStore identityStore, IUserController userClient, IUserMapper userMapper)
        : base(userClient, userMapper)
    {
        _identityStore = identityStore;
    }

    public Task<CurrentUserModel> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var identity = _identityStore.GetIdentity();
        var expiration = _identityStore.GetLoginExpiration();

        if (identity == null) return Task.FromResult<CurrentUserModel>(null);

        return Task.FromResult(new CurrentUserModel
        {
            Id = identity.UserId,
            Login = identity.Login,
            LoginExpiration = expiration,
            SessionId = identity.SessionId,
            UserType = identity.UserType
        });
    }
}
