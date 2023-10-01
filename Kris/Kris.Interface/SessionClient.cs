using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Kris.Interface
{
    public class SessionClient : ClientBase, ISessionController
    {
        public SessionClient() : base()
        {
        }

        public ActionResult<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public ActionResult UpdateUserName(UpdateUserNameRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
