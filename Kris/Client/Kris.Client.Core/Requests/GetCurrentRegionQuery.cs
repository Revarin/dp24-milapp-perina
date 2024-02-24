using MediatR;
using Microsoft.Maui.Maps;

namespace Kris.Client.Core.Requests;

public sealed class GetCurrentRegionQuery : IRequest<MapSpan>
{
}
