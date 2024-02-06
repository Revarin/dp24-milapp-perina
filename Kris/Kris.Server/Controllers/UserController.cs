using Microsoft.AspNetCore.Mvc;
using MediatR;
using Kris.Interface.Interfaces;
using Kris.Interface.Requests;

namespace Kris.Server.Controllers;

public sealed class UserController : ControllerBase, IUserController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<ActionResult<object>> RegisterUser(RegisterUserRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<object>> LoginUser(LoginUserRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<object>> StoreUserSettings(StoreUserSettingsRequest request)
    {
        throw new NotImplementedException();
    }
}
