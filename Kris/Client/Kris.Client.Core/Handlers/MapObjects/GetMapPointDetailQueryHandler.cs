using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using MediatR;

namespace Kris.Client.Core.Handlers.MapObjects;

public sealed class GetMapPointDetailQueryHandler : MapObjectsHandler, IRequestHandler<GetMapPointDetailQuery, Result<MapPointDetailModel>>
{
    private readonly IMapObjectsMapper _mapObjectsMapper;

    public GetMapPointDetailQueryHandler(IMapObjectsMapper mapObjectsMapper,
        IMapObjectController mapObjectClient)
        : base(mapObjectClient)
    {
        _mapObjectsMapper = mapObjectsMapper;
    }

    public async Task<Result<MapPointDetailModel>> Handle(GetMapPointDetailQuery request, CancellationToken cancellationToken)
    {
        var response = await _mapObjectClient.GetMapPoint(request.PointId, cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok(_mapObjectsMapper.MapPoint(response.Value));
    }
}
