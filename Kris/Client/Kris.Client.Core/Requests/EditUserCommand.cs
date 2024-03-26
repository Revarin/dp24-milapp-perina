using FluentResults;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class EditUserCommand : IRequest<Result>
{
    public string NewPassword { get; set; }
    public string Password { get; set; }
}
