using FluentResults;
using Kris.Client.Core.Models;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class GetMapPointDetailQuery : IRequest<Result<MapPointDetailModel>>
{
    public Guid PointId { get; set; }
}
