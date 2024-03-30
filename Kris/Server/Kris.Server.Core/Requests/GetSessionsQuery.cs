using FluentResults;
using Kris.Interface.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class GetSessionsQuery : IRequest<Result<IEnumerable<SessionListModel>>>
{
}
