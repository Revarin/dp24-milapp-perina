using Kris.Client.Core.Requests;
using Kris.Client.Core.Services;
using MediatR;

namespace Kris.Client.Core.Handlers.Map;

public sealed class GetCurrentPositionQueryHandler : MapHandler, IRequestHandler<GetCurrentPositionQuery, Location>
{
    private readonly IGpsService _gpsService;

    public GetCurrentPositionQueryHandler(IGpsService gpsService)
    {
        _gpsService = gpsService;
    }

    public async Task<Location> Handle(GetCurrentPositionQuery request, CancellationToken cancellationToken)
    {
        return await _gpsService.GetLastLocationAsync();
    }
}
