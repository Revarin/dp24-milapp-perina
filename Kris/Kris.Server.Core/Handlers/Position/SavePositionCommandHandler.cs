﻿using FluentResults;
using Kris.Common.Models;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Models;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Position;

public sealed class SavePositionCommandHandler : PositionHandler, IRequestHandler<SavePositionCommand, Result>
{
    public SavePositionCommandHandler(IUserPositionRepository positionRepository, IPositionMapper positionMapper, IAuthorizationService authorizationService)
        : base(positionRepository, positionMapper, authorizationService)
    {
    }

    public async Task<Result> Handle(SavePositionCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var authorized = await _authorizationService.AuthorizeAsync(user, cancellationToken);
        if (!authorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType));

        var position = await _positionRepository.GetAsync(user.Id, user.SessionId.Value, cancellationToken);
        if (position == null)
        {
            position = new UserPositionEntity
            {
                UserId = user.Id,
                SessionId = user.SessionId.Value,
                Updated = DateTime.UtcNow,
                Position_0 = request.SavePosition.Position,
                Position_1 = null,
                Position_2 = null,
            };
            var entity = await _positionRepository.InsertAsync(position, cancellationToken);
            if (entity == null) throw new DatabaseException("Failed to insert user position");
        }
        else
        {
            position.Position_2 = position.Position_1;
            position.Position_1 = position.Position_0;
            position.Position_0 = request.SavePosition.Position;
            position.Updated = DateTime.UtcNow;
            var updated = await _positionRepository.UpdateAsync(position, cancellationToken);
            if (!updated) throw new DatabaseException("Failed to update position");
        }

        return Result.Ok();
    }
}
