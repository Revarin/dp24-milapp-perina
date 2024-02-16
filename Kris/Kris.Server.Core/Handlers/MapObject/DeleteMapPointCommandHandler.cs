using FluentResults;
using Kris.Common.Enums;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.MapObject;

public sealed class DeleteMapPointCommandHandler : MapObjectHandler, IRequestHandler<DeleteMapPointCommand, Result>
{
    private readonly IMapPointRepository _mapPointRepository;

    public DeleteMapPointCommandHandler(IMapPointRepository mapPointRepository, IAuthorizationService authorizationService)
        : base(authorizationService)
    {
        _mapPointRepository = mapPointRepository;
    }

    public async Task<Result> Handle(DeleteMapPointCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        var mapPoint = await _mapPointRepository.GetAsync(request.MapPointId, cancellationToken);
        if (mapPoint == null) return Result.Fail(new EntityNotFoundError("MapPoint", request.MapPointId));

        var minRole = user.Id == mapPoint.UserId ? UserType.Basic : UserType.Admin;
        var authorized = await _authorizationService.AuthorizeAsync(user, minRole, cancellationToken);
        if (!authorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var deleted = await _mapPointRepository.DeleteAsync(request.MapPointId, cancellationToken);
        if (!deleted) throw new DatabaseException("Failed to delete map point");

        return Result.Ok();
    }
}
