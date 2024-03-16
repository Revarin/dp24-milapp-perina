using FluentResults;
using Kris.Interface.Models;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.MapObject;

public sealed class GetMapPointQueryHandler : MapObjectHandler, IRequestHandler<GetMapPointQuery, Result<MapPointDetailModel>>
{
    private readonly IMapPointRepository _mapPointRepository;
    private readonly IMapObjectMapper _mapObjectMapper;

    public GetMapPointQueryHandler(IMapPointRepository mapPointRepository, IMapObjectMapper mapObjectMapper,
        IAuthorizationService authorizationService)
        : base(authorizationService)
    {
        _mapPointRepository = mapPointRepository;
        _mapObjectMapper = mapObjectMapper;
    }

    public async Task<Result<MapPointDetailModel>> Handle(GetMapPointQuery request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));
        var authResult = await _authorizationService.AuthorizeAsync(user, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var mapPoint = await _mapPointRepository.GetWithAllAsync(request.PointId, cancellationToken);
        if (mapPoint == null) return Result.Fail(new EntityNotFoundError("MapPoint", request.PointId));

        return Result.Ok(_mapObjectMapper.MapPointDetail(mapPoint));
    }
}
