using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using MediatR;

namespace Kris.Client.Core.Handlers.User;

public sealed class LogoutUserCommandHandler : UserHandler, IRequestHandler<LogoutUserCommand>
{
    private readonly IIdentityStore _identityStore;

    public LogoutUserCommandHandler(IIdentityStore identityStore, IUserController userClient, IUserMapper userMapper)
        : base(userClient, userMapper)
    {
        _identityStore = identityStore;
    }

    public Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        _identityStore.ClearIdentity();
        return Task.CompletedTask;
    }
}
