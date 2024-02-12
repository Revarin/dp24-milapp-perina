using FluentResults;
using Kris.Interface.Requests;
using Kris.Server.Common.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class CreateSessionCommand : AuthentizedRequest, IRequest<Result<JwtToken>>
{
    public required CreateSessionRequest CreateSession { get; set; }
}
