using FluentResults;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class CreateSessionCommand : AuthentizedRequest, IRequest<Result<IdentityResponse>>
{
    public required CreateSessionRequest CreateSession { get; set; }
}
