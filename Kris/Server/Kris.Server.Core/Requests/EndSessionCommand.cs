using FluentResults;
using Kris.Interface.Responses;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class EndSessionCommand : AuthentizedRequest, IRequest<Result>
{
}
