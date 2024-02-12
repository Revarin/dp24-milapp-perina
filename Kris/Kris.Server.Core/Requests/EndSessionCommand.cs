using FluentResults;
using Kris.Server.Common.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class EndSessionCommand : AuthentizedRequest, IRequest<Result<JwtToken>>
{
}
