using FluentResults;
using Kris.Common.Enums;
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
        if (request.User.UserType > UserType.Basic) session.Users = entity.Users.Select(user => new SessionUserModel
        {
            Id = user.UserId,
            Login = user.User!.Login,
            Nickname = user.Nickname,
            UserType = user.UserType,
            Joined = user.Joined
        });

        return Result.Ok(session);
    }
}
