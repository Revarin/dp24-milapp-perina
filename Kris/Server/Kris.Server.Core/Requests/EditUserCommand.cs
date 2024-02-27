using FluentResults;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class EditUserCommand : AuthentizedRequest, IRequest<Result<IdentityResponse>>
{
    public required EditUserRequest EditUser { get; set; }
}
