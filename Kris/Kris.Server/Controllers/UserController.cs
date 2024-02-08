using Microsoft.AspNetCore.Mvc;
using MediatR;
using Kris.Interface.Interfaces;
using Kris.Interface.Requests;
using Kris.Server.Core.Requests;
using Kris.Server.Common.Errors;

namespace Kris.Server.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/user")]
public sealed class UserController : ControllerBase, IUserController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> RegisterUser(RegisterUserRequest request, CancellationToken ct)
    {
        var commmand = new RegisterUserCommand { RegisterUser = request };
        var result = await _mediator.Send(commmand, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UserExistsError>()) return BadRequest(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> LoginUser(LoginUserRequest request, CancellationToken ct)
    {
        var command = new LoginUserCommand { LoginUser = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<InvalidCredentialsError>()) return Unauthorized(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok();
    }

    // TODO
    public Task<ActionResult<object>> StoreUserSettings(StoreUserSettingsRequest request)
    {
        throw new NotImplementedException();
    }
}
