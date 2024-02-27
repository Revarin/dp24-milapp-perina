using FluentResults;
using Kris.Interface.Responses;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class LeaveSessionCommand : AuthentizedRequest, IRequest<Result<IdentityResponse>>
{
    public required Guid SessionId { get; set; }
}
