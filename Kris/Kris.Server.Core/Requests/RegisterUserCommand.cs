using FluentResults;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed record RegisterUserCommand : IRequest<Result>
{
    public required RegisterUserRequest RegisterUser { get; set; }
}
