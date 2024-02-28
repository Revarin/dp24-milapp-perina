using FluentResults;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class JoinSessionCommand : IRequest<Result>
{
    public Guid SessionId { get; set; }
    public string Password { get; set; }
}
