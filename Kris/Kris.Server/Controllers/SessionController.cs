using Microsoft.AspNetCore.Mvc;
using Kris.Interface;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Kris.Server
{
    public class SessionController : ControllerBase, ISessionController
    {
        private readonly IUserService _userService;

        public SessionController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            if (string.IsNullOrEmpty(request.Name)) return BadRequest("Missing user name");

            var newUser = _userService.CreateUser(request.Name);

            if (newUser == null) return Conflict(_userService.GetErrorMessage());

            return Ok(newUser);
        }

        [HttpPut]
        public ActionResult UpdateUserName(UpdateUserNameRequest request)
        {
            if (string.IsNullOrEmpty(request.Name)) return BadRequest("Missing user name");

            var result = _userService.UpdateUserName(request.Id, request.Name);

            if (result) return Ok();
            else return Conflict(_userService.GetErrorMessage());
        }
    }
}
