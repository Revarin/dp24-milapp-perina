using FluentResults;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class EditSessionCommand : AuthentizedRequest, IRequest<Result>
{
    public required EditSessionRequest EditSession { get; set; }
}
