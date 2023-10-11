using Kris.Server.Data;

namespace Kris.Server
{
    public interface IUserService : IServiceBase
    {
        UserEntity CreateUser(string name);
        bool UpdateUserName(int id, string name);
        bool UserExists(int id);
    }
}
