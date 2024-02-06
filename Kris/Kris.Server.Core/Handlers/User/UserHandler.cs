using AutoMapper;
using Kris.Server.Data.Repositories;

namespace Kris.Server.Core.Handlers.User;

public abstract class UserHandler : BaseHandler
{
    protected IUserRepository _userRepository;

    protected UserHandler(IUserRepository userRepository, IMapper mapper) : base(mapper)
    {
        _userRepository = userRepository;
    }
}
