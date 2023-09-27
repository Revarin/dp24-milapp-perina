using Kris.Server.Data;

namespace Kris.Server
{
    public interface IUserService
    {
        UserEntity CreateUser(string name);
        bool UpdateUserName(int id, string name);
    }
}
