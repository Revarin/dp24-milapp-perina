﻿using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Mappers;
using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

namespace Kris.Client.Core.Handlers.Session;

public sealed class EditSessionUserCommandHandler : SessionHandler, IRequestHandler<EditSessionUserCommand, Result>
{
    public EditSessionUserCommandHandler(ISessionController sessionClient, IIdentityStore identityStore, IUserMapper userMapper)
        : base(sessionClient, identityStore, userMapper)
    {
    }

    public async Task<Result> Handle(EditSessionUserCommand request, CancellationToken cancellationToken)
    {
        using var t = Common.Metrics.SentryMetrics.TimerStart("RequestHandler");
        var httpRequest = new EditSessionUserRequest
        {
            Nickname = request.UserName,
            Symbol = request.UserSymbol
        };
        IdentityResponse response;

        try
        {
            response = await _sessionClient.EditSessionUser(httpRequest, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else return Result.Fail(new ServerError(response.Message));
        }

        _identityStore.StoreIdentity(_userMapper.Map(response));

        return Result.Ok();
    }
}
