using Kris.Client.Core.Requests;
using Kris.Client.Data.Cache;
using MediatR;
using Microsoft.Maui.Maps;

namespace Kris.Client.Core.Handlers.Map;

public sealed class GetCurrentRegionQueryHandler : MapHandler, IRequestHandler<GetCurrentRegionQuery, MapSpan>
{
    private readonly ILocationStore _locationStore;

    public GetCurrentRegionQueryHandler(ILocationStore locationStore)
    {
        _locationStore = locationStore;
    }

    public Task<MapSpan> Handle(GetCurrentRegionQuery request, CancellationToken cancellationToken)
    {
        var region = _locationStore.GetCurrentRegion();
        return Task.FromResult(region);
    }
}
