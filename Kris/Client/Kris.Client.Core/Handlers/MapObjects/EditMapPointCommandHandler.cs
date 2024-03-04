using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Requests;
using Kris.Common.Models;
using Kris.Interface.Controllers;
using Kris.Interface.Requests;
using MediatR;

namespace Kris.Client.Core.Handlers.MapObjects;

public sealed class EditMapPointCommandHandler : MapObjectsHandler, IRequestHandler<EditMapPointCommand, Result>
{
    public EditMapPointCommandHandler(IMapObjectController mapObjectClient)
        : base(mapObjectClient)
    {
    }

    public async Task<Result> Handle(EditMapPointCommand request, CancellationToken cancellationToken)
    {
        var httpRequest = new EditMapPointRequest
        {
            Id = request.PointId,
            Name = request.Name,
            Description = request.Description,
            Position = new GeoPosition
            {
                Latitude = request.Location.Latitude,
                Longitude = request.Location.Longitude,
                Altitude = request.Location.Altitude.GetValueOrDefault()
            },
            Symbol = new MapPointSymbol
            {
                Shape = request.Shape,
                Color = request.Color,
                Sign = request.Sign
            }
        };
        var response = await _mapObjectClient.EditMapPoint(httpRequest, cancellationToken);

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok();
    }
}
