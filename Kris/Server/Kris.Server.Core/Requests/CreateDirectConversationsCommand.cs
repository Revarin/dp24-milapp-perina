﻿using FluentResults;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class CreateDirectConversationsCommand : IRequest<Result>
{
    public required Guid SessionId { get; set; }
    public required Guid UserId { get; set; }
}
