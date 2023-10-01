using Microsoft.AspNetCore.Mvc;

namespace Kris.Interface
{
    public interface ISessionController
    {
        ActionResult<CreateUserResponse> CreateUser(CreateUserRequest request);
        ActionResult UpdateUserName(UpdateUserNameRequest request);
    }
}
