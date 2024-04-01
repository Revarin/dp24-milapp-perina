using FluentResults;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class KickSessionUserCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
}
