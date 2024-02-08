﻿using FluentResults;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Common.Models;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Session;

public sealed class CreateSessionCommandHandler : SessionHandler, IRequestHandler<CreateSessionCommand, Result<JwtToken>>
{
    private readonly IJwtService _jwtService;

    public CreateSessionCommandHandler(IJwtService jwtService, ISessionRepository sessionRepository, ISessionMapper sessionMapper)
        : base(sessionRepository, sessionMapper)
    {
        _jwtService = jwtService;
    }

    public async Task<Result<JwtToken>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var name = request.CreateSession.Name;

        var sessionExists = await _sessionRepository.SessionExistsAsync(name, cancellationToken);
        if (sessionExists) return Result.Fail(new EntityExistsError("Session", name));

        var session = _sessionMapper.Map(request);
        var sessionEntity = await _sessionRepository.InsertAsync(session, cancellationToken);
        if (sessionEntity == null) throw new Exception("Failed to insert entity");

        var jwt = _jwtService.CreateToken(request.User, sessionEntity, Kris.Common.Enums.UserType.SuperAdmin);
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create JWT token");

        return Result.Ok(jwt);
    }
}
