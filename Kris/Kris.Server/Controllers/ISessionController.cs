using Kris.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Server
{
    public interface ISessionController
    {
        ActionResult<CreateUserResponse> CreateUser(CreateUserRequest request);
        ActionResult UpdateUserName(UpdateUserNameRequest request);
    }
}
