using Kris.Client.Core.Mappers;
using Kris.Interface.Controllers;

namespace Kris.Client.Core.Handlers.User;

public abstract class UserHandler
{
    protected readonly IUserController _userClient;
    protected readonly IUserMapper _userMapper;

    protected UserHandler(IUserController userClient, IUserMapper userMapper)
    {
        _userClient = userClient;
        _userMapper = userMapper;
    }
}
