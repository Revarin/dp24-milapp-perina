using Kris.Interface;

namespace Kris.Server
{
    public interface IUserController
    {
        IResult CreateUser(CreateUserRequest request);
        IResult UpdateUserName(UpdateUserNameRequest request);
    }
}
