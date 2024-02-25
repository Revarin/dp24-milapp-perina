using FluentResults;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class KickFromSessionCommand : AuthentizedRequest, IRequest<Result>
{
    public required Guid UserId { get; set; }
}
