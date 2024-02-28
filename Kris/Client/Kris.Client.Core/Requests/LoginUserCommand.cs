using FluentResults;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class LoginUserCommand : IRequest<Result>
{
    public string Login { get; set; }
    public string Password { get; set; }
}
