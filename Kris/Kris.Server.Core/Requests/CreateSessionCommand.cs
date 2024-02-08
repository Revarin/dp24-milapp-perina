using FluentResults;
using Kris.Interface.Requests;
using Kris.Server.Common.Models;
using Kris.Server.Core.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class CreateSessionCommand : IRequest<Result<JwtToken>>
{
    public required CreateSessionRequest CreateSession { get; set; }
    public required UserModel User { get; set; }
}
