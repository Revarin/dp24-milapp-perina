using MediatR;

namespace Kris.Client.Core.Requests;

public class GetCurrentPositionQuery : IRequest<Location>
{
}
