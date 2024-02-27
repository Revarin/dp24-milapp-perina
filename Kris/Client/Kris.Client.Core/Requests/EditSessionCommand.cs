using FluentResults;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class EditSessionCommand : IRequest<Result>
{
    public string NewName { get; set; }
    public string NewPassword { get; set; }
    public string Password { get; set; }
}
