using FluentResults;
using Kris.Client.Core.Models;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class GetSessionDetailQuery : IRequest<Result<SessionDetailModel>>
{
    public Guid SessionId { get; set; }
}
