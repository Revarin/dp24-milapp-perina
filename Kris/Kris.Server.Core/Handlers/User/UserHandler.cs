using Kris.Server.Core.Mappers;
using Kris.Server.Data.Repositories;

namespace Kris.Server.Core.Handlers.User;

public abstract class UserHandler : BaseHandler
{
    protected readonly IUserRepository _userRepository;
    protected readonly IUserMapper _userMapper;

    protected UserHandler(IUserRepository userRepository, IUserMapper mapper)
    {
        _userRepository = userRepository;
        _userMapper = mapper;
    }
}
