using Microsoft.AspNetCore.Mvc;
using MediatR;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Server.Core.Requests;
using Kris.Server.Common.Errors;
using Microsoft.AspNetCore.Authorization;
using Kris.Interface.Responses;

namespace Kris.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class UserController : KrisController, IUserController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<ActionResult> RegisterUser(RegisterUserRequest request, CancellationToken ct)
    {
        var commmand = new RegisterUserCommand { RegisterUser = request };
        var result = await _mediator.Send(commmand, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityExistsError>()) return BadRequest(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok();
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<JwtTokenResponse>> LoginUser(LoginUserRequest request, CancellationToken ct)
    {
        var command = new LoginUserCommand { LoginUser = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<InvalidCredentialsError>()) return Unauthorized(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok(new JwtTokenResponse { Token = result.Value.Token });
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<JwtTokenResponse>> EditUser(EditUserRequest request, CancellationToken ct)
    {
        // Edit SELF ONLY
        var user = CurrentUser();
        if (user == null) return Unauthorized();

        var command = new EditUserCommand { User = user, EditUser = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Unauthorized(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok(new JwtTokenResponse { Token = result.Value.Token });
    }

    [HttpDelete()]
    [Authorize]
    public async Task<ActionResult> DeleteUser(CancellationToken ct)
    {
        // Delete SELF ONLY
        var user = CurrentUser();
        if (user == null) return Unauthorized();

        var command = new DeleteUserCommand { User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Unauthorized(result.Errors.Select(e => e.Message));
            else return BadRequest();
        }

        return Ok();
        throw new NotImplementedException();
    }

    // TODO
    [HttpPost("Settings")]
    [Authorize]
    public Task<ActionResult<object>> StoreUserSettings(StoreUserSettingsRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
