﻿using FluentResults;
using Kris.Interface.Models;
using Kris.Server.Common.Errors;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Session;

public sealed class GetSessionQueryHandler : SessionHandler, IRequestHandler<GetSessionQuery, Result<SessionDetailModel>>
{
    public GetSessionQueryHandler(ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
    }

    public async Task<Result<SessionDetailModel>> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        var entity = await _sessionRepository.GetWithAllAsync(request.SessionId, cancellationToken);
        if (entity == null) return Result.Fail(new EntityNotFoundError("Session", request.SessionId));

        var session = _sessionMapper.MapDetail(entity, request.User.UserId);
        return Result.Ok(session);
    }
}
