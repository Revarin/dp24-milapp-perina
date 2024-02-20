using FluentResults;
using Kris.Client.Core.Models;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class GetSessionsQuery : IRequest<Result<AvailableSessionsModel>>
{
}
