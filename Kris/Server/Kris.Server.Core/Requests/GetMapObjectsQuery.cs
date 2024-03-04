using FluentResults;
using Kris.Interface.Responses;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class GetMapObjectsQuery : AuthentizedRequest, IRequest<Result<GetMapObjectsResponse>>
{
    public DateTime? From { get; set; }
}
