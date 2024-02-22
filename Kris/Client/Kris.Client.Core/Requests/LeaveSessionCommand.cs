using FluentResults;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class LeaveSessionCommand : IRequest<Result>
{
    public Guid SessionId { get; set; }
}
