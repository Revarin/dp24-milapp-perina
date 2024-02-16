using FluentResults;
using Kris.Interface.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class GetSessionQuery : IRequest<Result<SessionModel>>
{
    public required Guid SessionId { get; set; }
}
