using FluentResults;
using Kris.Server.Common.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public class JoinSessionCommand : AuthentizedRequest, IRequest<Result<JwtToken>>
{
    public required Guid SessionId { get; set; }
}
