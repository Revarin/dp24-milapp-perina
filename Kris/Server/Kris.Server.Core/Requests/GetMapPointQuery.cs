using FluentResults;
using Kris.Interface.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class GetMapPointQuery : AuthentizedRequest, IRequest<Result<MapPointDetailModel>>
{
    public required Guid PointId { get; set; }
}
