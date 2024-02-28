using FluentResults;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class EditSessionCommand : AuthentizedRequest, IRequest<Result>
{
    public required EditSessionRequest EditSession { get; set; }
}
