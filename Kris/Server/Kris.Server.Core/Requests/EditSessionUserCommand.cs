using FluentResults;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class EditSessionUserCommand : AuthentizedRequest, IRequest<Result<IdentityResponse>>
{
    public required EditSessionUserRequest EditSessionUser { get; set; }
}
