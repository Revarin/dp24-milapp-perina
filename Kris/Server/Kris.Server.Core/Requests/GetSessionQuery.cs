using FluentResults;
using Kris.Interface.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class GetSessionQuery : AuthentizedRequest, IRequest<Result<SessionDetailModel>>
{
    public required Guid SessionId { get; set; }
}
