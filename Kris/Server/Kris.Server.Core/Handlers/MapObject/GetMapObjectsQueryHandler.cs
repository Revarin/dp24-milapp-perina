using FluentResults;
using Kris.Interface.Responses;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.MapObject;

public sealed class GetMapObjectsQueryHandler : MapObjectHandler, IRequestHandler<GetMapObjectsQuery, Result<GetMapObjectsResponse>>
{
    private readonly IMapPointRepository _mapPointRepository;
    private readonly IMapObjectMapper _mapObjectMapper;

    public GetMapObjectsQueryHandler(IMapPointRepository mapPointRepository, IMapObjectMapper mapObjectMapper,
        IAuthorizationService authorizationService)
        : base(authorizationService)
    {
        _mapPointRepository = mapPointRepository;
        _mapObjectMapper = mapObjectMapper;
    }

    public async Task<Result<GetMapObjectsResponse>> Handle(GetMapObjectsQuery request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));
        var authResult = await _authorizationService.AuthorizeAsync(user, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var mapPoints = request.From == null
            ? await _mapPointRepository.GetWithUsersAsync(user.SessionId.Value, cancellationToken)
            : await _mapPointRepository.GetWithUsersAsync(user.SessionId.Value, request.From.Value, cancellationToken);

        var response = new GetMapObjectsResponse
        {
            Resolved = DateTime.UtcNow,
            MapPoints = mapPoints.Select(_mapObjectMapper.MapPoint).ToList()
        };
        return Result.Ok(response);
    }
}
