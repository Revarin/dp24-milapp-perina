using FluentResults;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Server.Core.Requests;

public class LoginUserCommand : IRequest<Result>
{
    public required LoginUserRequest LoginUser { get; set; }
}
