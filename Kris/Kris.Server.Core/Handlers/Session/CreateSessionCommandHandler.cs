using FluentResults;
using Kris.Common.Enums;
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

    public CreateSessionCommandHandler(IJwtService jwtService, ISessionRepository sessionRepository, ISessionMapper sessionMapper, IAuthorizationService authorizationService)
        : base(sessionRepository, sessionMapper, authorizationService)
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
        if (sessionEntity == null) throw new DatabaseException("Failed to insert Session");

        var jwt = _jwtService.CreateToken(request.User, sessionEntity, UserType.SuperAdmin);
        if (string.IsNullOrEmpty(jwt.Token)) throw new JwtException("Failed to create token");

        return Result.Ok(jwt);
    }
}
