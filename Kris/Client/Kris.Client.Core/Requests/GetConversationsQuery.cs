using FluentResults;
using Kris.Client.Core.Models;
using MediatR;

namespace Kris.Client.Core.Requests;

public sealed class GetConversationsQuery : IRequest<Result<IEnumerable<ConversationModel>>>
{
}
