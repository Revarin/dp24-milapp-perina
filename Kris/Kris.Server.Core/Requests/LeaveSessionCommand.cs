using FluentResults;
using Kris.Server.Common.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class LeaveSessionCommand : AuthentizedRequest, IRequest<Result<JwtToken>>
{
    public required Guid SessionId { get; set; }
}
