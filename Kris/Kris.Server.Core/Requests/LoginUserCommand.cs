using FluentResults;
using Kris.Interface.Requests;
using Kris.Server.Common.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public class LoginUserCommand : IRequest<Result<JwtToken>>
{
    public required LoginUserRequest LoginUser { get; set; }
}
