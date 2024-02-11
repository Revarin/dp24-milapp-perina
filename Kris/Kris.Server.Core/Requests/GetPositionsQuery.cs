using FluentResults;
using Kris.Interface.Responses;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class GetPositionsQuery : AuthentizedRequest, IRequest<Result<GetPositionsResponse>>
{
    public DateTime? From { get; set; }
}
