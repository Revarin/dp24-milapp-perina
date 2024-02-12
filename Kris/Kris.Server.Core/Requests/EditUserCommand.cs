using FluentResults;
using Kris.Interface.Requests;
using Kris.Server.Common.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class EditUserCommand : AuthentizedRequest, IRequest<Result<JwtToken>>
{
    public required EditUserRequest EditUser { get; set; }
}
