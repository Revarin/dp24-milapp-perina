﻿using FluentResults;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;

namespace Kris.Server.Core.Requests;

public class LoginUserCommand : IRequest<Result<LoginSettingsResponse>>
{
    public required LoginUserRequest LoginUser { get; set; }
}
