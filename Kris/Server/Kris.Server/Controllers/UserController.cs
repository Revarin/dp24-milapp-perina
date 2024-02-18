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
    public async Task<Response<EmptyResponse>> RegisterUser(RegisterUserRequest request, CancellationToken ct)
    {
        var commmand = new RegisterUserCommand { RegisterUser = request };
        var result = await _mediator.Send(commmand, ct);

        if (result.IsFailed)
        {
            if (result.HasError<EntityExistsError>()) return Response.BadRequest<EmptyResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<EmptyResponse>();
        }

        return Response.Ok<EmptyResponse>();
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<Response<LoginResponse>> LoginUser(LoginUserRequest request, CancellationToken ct)
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
    public async Task<Response<LoginResponse>> EditUser(EditUserRequest request, CancellationToken ct)
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
    public async Task<Response<EmptyResponse>> DeleteUser(CancellationToken ct)
    {
        // Delete SELF ONLY
        var user = CurrentUser();
        if (user == null) return Response.Unauthorized<EmptyResponse>();

        var command = new DeleteUserCommand { User = user };
        var result = await _mediator.Send(command, ct);

        if (result.IsFailed)
        {
            if (result.HasError<UnauthorizedError>()) return Response.Unauthorized<EmptyResponse>(result.Errors.FirstMessage());
            else return Response.InternalError<EmptyResponse>();
        }

        return Response.Ok<EmptyResponse>();
    }

    // TODO
    [HttpPost("Settings")]
    [Authorize]
    public Task<Response<EmptyResponse>> StoreUserSettings(StoreUserSettingsRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
