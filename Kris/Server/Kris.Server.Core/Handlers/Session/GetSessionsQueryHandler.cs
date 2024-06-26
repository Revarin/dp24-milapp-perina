﻿using FluentResults;
using Kris.Interface.Models;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Session;

public sealed class GetSessionsQueryHandler : SessionHandler, IRequestHandler<GetSessionsQuery, Result<IEnumerable<SessionListModel>>>
{
    public GetSessionsQueryHandler(ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
    {
    }

    public async Task<Result<IEnumerable<SessionListModel>>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _sessionRepository.GetWithAllAsync(cancellationToken);
        var sessions = entities.Select(_sessionMapper.MapList);
        return Result.Ok(sessions);
    }
}
