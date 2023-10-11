using Microsoft.AspNetCore.Mvc;
using Kris.Interface;

namespace Kris.Server
{
    [Route("[controller]/[action]")]
    public class SessionController : ControllerBase, ISessionController
    {
        private readonly IUserService _userService;

        public SessionController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public Task<CreateUserResponse> CreateUser([FromBody]CreateUserRequest request)
        {
            if (request == null) throw new BadHttpRequestException("Missing request body");
            if (string.IsNullOrEmpty(request.Name)) throw new BadHttpRequestException("Missing request body");

            var newUser = _userService.CreateUser(request.Name);

            if (newUser == null) throw new BadHttpRequestException(_userService.GetErrorMessage());

            return Task.FromResult(new CreateUserResponse
            {
                Id = newUser.Id,
                Name = newUser.Name,
                CreatedDate = newUser.CreatedDate
            });
        }

        [HttpPut]
        public Task UpdateUserName([FromBody]UpdateUserNameRequest request)
        {
            if (request == null) throw new BadHttpRequestException("Missing request body");
            if (string.IsNullOrEmpty(request.Name)) throw new BadHttpRequestException("Missing request body");

            var result = _userService.UpdateUserName(request.Id, request.Name);

            if (!result) throw new BadHttpRequestException(_userService.GetErrorMessage());

            return Task.CompletedTask;
        }

        [HttpPost]
        public Task<bool> UserExists([FromBody]UserExistsRequest request)
        {
            if (request == null) throw new BadHttpRequestException("Missing request body");

            var result = _userService.UserExists(request.Id);

            return Task.FromResult(result);
        }
    }
}
