using FluentResults;
using Kris.Interface.Requests;
using Kris.Server.Common.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public class JoinSessionCommand : AuthentizedRequest, IRequest<Result<JwtToken>>
{
    public required JoinSessionRequest JoinSession { get; set; }
}
