﻿using FluentResults;
using Kris.Common.Enums;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.MapObject;

public sealed class EditMapPointCommandHandler : MapObjectHandler, IRequestHandler<EditMapPointCommand, Result>
{
    private readonly IMapPointRepository _mapPointRepository;

    public EditMapPointCommandHandler(IMapPointRepository mapPointRepository, IAuthorizationService authorizationService)
        : base(authorizationService)
    {
        _mapPointRepository = mapPointRepository;
    }

    public async Task<Result> Handle(EditMapPointCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        var mapPoint = await _mapPointRepository.GetWithUserAsync(request.EditMapPoint.Id, cancellationToken);
        if (mapPoint == null) return Result.Fail(new EntityNotFoundError("MapPoint", request.EditMapPoint.Id));

        var minRole = user.UserId == mapPoint.SessionUser!.UserId ? UserType.Basic : UserType.Admin;
        var authResult = await _authorizationService.AuthorizeAsync(user, minRole, cancellationToken);
        if (!authResult.IsAuthorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        mapPoint.Name = request.EditMapPoint.Name;
        mapPoint.Description = request.EditMapPoint.Description;
        mapPoint.Type = request.EditMapPoint.Type;
        mapPoint.Position = request.EditMapPoint.Position;
        mapPoint.Symbol = request.EditMapPoint.Symbol;
        mapPoint.Created = DateTime.UtcNow;
        await _mapPointRepository.UpdateAsync(cancellationToken);

        return Result.Ok();
    }
}