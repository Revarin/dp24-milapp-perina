using Microsoft.AspNetCore.Mvc;
using Kris.Interface;

namespace Kris.Server
{
    public class UserController : ControllerBase, IUserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IResult CreateUser(CreateUserRequest request)
        {
            var newUser = _userService.CreateUser(request.Name);

            if (newUser == null) return Results.Conflict();

            return Results.Ok(newUser);
        }

        public IResult UpdateUserName(UpdateUserNameRequest request)
        {
            var result = _userService.UpdateUserName(request.Id, request.Name);

            if (result) return Results.Ok();
            else return Results.Conflict();
        }
    }
}
