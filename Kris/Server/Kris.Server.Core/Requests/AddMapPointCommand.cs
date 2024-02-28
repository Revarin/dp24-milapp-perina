using FluentResults;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class AddMapPointCommand : AuthentizedRequest, IRequest<Result<Guid>>
{
    public required AddMapPointRequest AddMapPoint { get; set; }
}
