using FluentResults;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class RegisterUserCommand : IRequest<Result>
{
    public string Login { get; set; }
    public string Password { get; set; }
}
