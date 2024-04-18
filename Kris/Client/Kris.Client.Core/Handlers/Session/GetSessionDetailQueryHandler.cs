using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

using ClientSessionDetailModel = Kris.Client.Core.Models.SessionDetailModel;
using InterfaceSessionDetailModel = Kris.Interface.Models.SessionDetailModel;
using ClientSessionUserModel = Kris.Client.Core.Models.SessionUserModel;

namespace Kris.Client.Core.Handlers.Session;

public sealed class GetSessionDetailQueryHandler : SessionHandler, IRequestHandler<GetSessionDetailQuery, Result<ClientSessionDetailModel>>
{
    public GetSessionDetailQueryHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    public async Task<Result<ClientSessionDetailModel>> Handle(GetSessionDetailQuery request, CancellationToken cancellationToken)
    {
        using var t = Common.Metrics.SentryMetrics.TimerStart("RequestHandler");
        GetOneResponse<InterfaceSessionDetailModel> response;

        try
        {
            response = await _sessionClient.GetSession(request.SessionId, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok(new ClientSessionDetailModel
        {
            Id = response.Value.Id,
            Name = response.Value.Name,
            Created = response.Value.Created,
            UserName = response.Value.UserName,
            UserSymbol = response.Value.UserSymbol,
            Users = response.Value.Users.Select(user => new ClientSessionUserModel
            {
                Id = user.Id,
                Login = user.Login,
                Nickname = user.Nickname,
                UserType = user.UserType,
                Joined = user.Joined
            })
        });
    }
}
