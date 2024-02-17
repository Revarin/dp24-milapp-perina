using Kris.Interface.Controllers;

namespace Kris.Client.Core.Handlers.User;

public abstract class UserHandler
{
    protected readonly IUserController _userClient;

    protected UserHandler(IUserController userClient)
    {
        _userClient = userClient;
    }
}
