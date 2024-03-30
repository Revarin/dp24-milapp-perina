using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Models;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using MediatR;

namespace Kris.Client.Core.Handlers.Session;

public sealed class GetSessionDetailQueryHandler : SessionHandler, IRequestHandler<GetSessionDetailQuery, Result<SessionDetailModel>>
{
    public GetSessionDetailQueryHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    public async Task<Result<SessionDetailModel>> Handle(GetSessionDetailQuery request, CancellationToken cancellationToken)
    {
        var response = await _sessionClient.GetSession(request.SessionId, cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok(new SessionDetailModel
        {
            Id = response.Value.Id,
            Name = response.Value.Name,
            Created = response.Value.Created,
            UserName = response.Value.UserName,
            UserSymbol = response.Value.UserSymbol
        });
    }
}
