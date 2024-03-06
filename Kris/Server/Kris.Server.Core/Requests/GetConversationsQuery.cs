using FluentResults;
using Kris.Interface.Models;
using MediatR;

namespace Kris.Server.Core.Requests;

public sealed class GetConversationsQuery : AuthentizedRequest, IRequest<Result<IEnumerable<ConversationListModel>>>
{
}
