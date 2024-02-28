using FluentResults;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class DeleteMapPointCommand : AuthentizedRequest, IRequest<Result>
{
    public required Guid MapPointId { get; set; }
}
