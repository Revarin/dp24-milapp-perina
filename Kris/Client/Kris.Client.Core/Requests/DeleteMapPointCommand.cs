using FluentResults;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class DeleteMapPointCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}
