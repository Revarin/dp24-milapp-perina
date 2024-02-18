using FluentResults;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;

namespace Kris.Server.Core.Requests;

public class JoinSessionCommand : AuthentizedRequest, IRequest<Result<LoginResponse>>
{
    public required JoinSessionRequest JoinSession { get; set; }
}
