﻿using FluentResults;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class DeleteUserCommand : AuthentizedRequest, IRequest<Result>
{
    public required string Password { get; set; }
}
