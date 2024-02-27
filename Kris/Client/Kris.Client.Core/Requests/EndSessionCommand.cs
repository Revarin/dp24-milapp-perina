using FluentResults;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class EndSessionCommand : IRequest<Result>
{
    public string Password { get; set; }
}
