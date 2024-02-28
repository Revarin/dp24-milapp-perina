using FluentResults;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class CreateSessionCommand : IRequest<Result>
{
    public string Name { get; set; }
    public string Password { get; set; }
}
