using FluentResults;
using Kris.Interface.Responses;
using Kris.Server.Common.Errors;
using Kris.Server.Common.Exceptions;
using Kris.Server.Core.Mappers;
using Kris.Server.Core.Requests;
using Kris.Server.Core.Services;
using Kris.Server.Data.Repositories;
using MediatR;

namespace Kris.Server.Core.Handlers.Position;

public sealed class GetPositionsQueryHandler : PositionHandler, IRequestHandler<GetPositionsQuery, Result<GetPositionsResponse>>
{
    public GetPositionsQueryHandler(IUserPositionRepository positionRepository, IPositionMapper positionMapper, IAuthorizationService authorizationService)
        : base(positionRepository, positionMapper, authorizationService)
    {
    }

    public async Task<Result<GetPositionsResponse>> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (!user.SessionId.HasValue) throw new JwtException("Token missing session");

        var authorized = await _authorizationService.AuthorizeAsync(user, cancellationToken);
        if (!authorized) return Result.Fail(new UnauthorizedError(user.Login, user.SessionName, user.UserType.ToString()));

        var positions = request.From == null
            ? await _positionRepository.GetWithUsersAsync(user.SessionId.Value, cancellationToken)
            : await _positionRepository.GetWithUsersAsync(user.SessionId.Value, request.From.Value, cancellationToken);

        var response = new GetPositionsResponse
        {
            Resolved = DateTime.UtcNow,
            UserPositions = positions.Select(_positionMapper.Map).ToList()
        };
        return Result.Ok(response);
    }
}
