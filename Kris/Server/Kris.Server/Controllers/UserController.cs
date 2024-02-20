using Microsoft.AspNetCore.Mvc;
using MediatR;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Server.Core.Requests;
using Kris.Server.Common.Errors;
using Microsoft.AspNetCore.Authorization;
using Kris.Interface.Responses;
using Kris.Server.Extensions;

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
    public async Task<Response?> RegisterUser(RegisterUserRequest request, CancellationToken ct)
    {
        var commmand = new RegisterUserCommand { RegisterUser = request };
        var result = await _mediator.Send(commmand, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityExistsError>()) return Response.BadRequest<Response>(result.Errors.FirstMessage());
            else return Response.InternalError<Response>();
        }

        return Response.Ok();
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<LoginResponse?> LoginUser(LoginUserRequest request, CancellationToken ct)
    {
        var command = new LoginUserCommand { LoginUser = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<InvalidCredentialsError>()) return Response.Unauthorized<LoginResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<LoginResponse>();
        }

        return Response.Ok(result.Value);
    }

    [HttpPut]
    [Authorize]
    public async Task<LoginResponse?> EditUser(EditUserRequest request, CancellationToken ct)
    {
        // Edit SELF ONLY
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<LoginResponse>();

        var command = new EditUserCommand { User = user, EditUser = request };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<LoginResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<LoginResponse>();
        }

        return Response.Ok(result.Value);
    }

    [HttpDelete()]
    [Authorize]
    public async Task<Response?> DeleteUser(CancellationToken ct)
    {
        // Delete SELF ONLY
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<Response>();

        var command = new DeleteUserCommand { User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<Response>(result.Errors.FirstMessage());
            else return Response.InternalError<Response>();
        }

        return Response.Ok();
    }

    // TODO
    [HttpPost("Settings")]
    [Authorize]
    public Task<Response?> StoreUserSettings(StoreUserSettingsRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
