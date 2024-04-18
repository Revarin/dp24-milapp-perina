using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

using ClientMapPointDetailModel = Kris.Client.Core.Models.MapPointDetailModel;
using InterfaceMapPointDetailModel = Kris.Interface.Models.MapPointDetailModel;

namespace Kris.Client.Core.Handlers.MapObjects;

public sealed class GetMapPointDetailQueryHandler : MapObjectsHandler, IRequestHandler<GetMapPointDetailQuery, Result<ClientMapPointDetailModel>>
{
    private readonly IMapObjectsMapper _mapObjectsMapper;

    public GetMapPointDetailQueryHandler(IMapObjectsMapper mapObjectsMapper,
        IMapObjectController mapObjectClient)
        : base(mapObjectClient)
    {
        _mapObjectsMapper = mapObjectsMapper;
    }

    public async Task<Result<ClientMapPointDetailModel>> Handle(GetMapPointDetailQuery request, CancellationToken cancellationToken)
    {
        using var t = Common.Metrics.SentryMetrics.TimerStart("RequestHandler");
        GetOneResponse<InterfaceMapPointDetailModel> response;

        try
        {
            response = await _mapObjectClient.GetMapPoint(request.PointId, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok(_mapObjectsMapper.MapPoint(response.Value));
    }
}
