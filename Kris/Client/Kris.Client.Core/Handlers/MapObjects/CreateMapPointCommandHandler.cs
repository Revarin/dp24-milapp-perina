using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Image;
using Kris.Client.Core.Requests;
using Kris.Common.Models;
using Kris.Interface.Controllers;
using Kris.Interface.Models;
using Kris.Interface.Requests;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

namespace Kris.Client.Core.Handlers.MapObjects;

public sealed class CreateMapPointCommandHandler : MapObjectsHandler, IRequestHandler<CreateMapPointCommand, Result<Guid>>
{
    private readonly IImageAttachmentComposer _imageAttachmentComposer;

    public CreateMapPointCommandHandler(IImageAttachmentComposer imageAttachmentComposer,
        IMapObjectController mapObjectClient)
        : base(mapObjectClient)
    {
        _imageAttachmentComposer = imageAttachmentComposer;
    }

    public async Task<Result<Guid>> Handle(CreateMapPointCommand request, CancellationToken cancellationToken)
    {
        using var t = Common.Metrics.SentryMetrics.TimerStart("RequestHandler");
        var httpRequest = new AddMapPointRequest
        {
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
            },
            Attachments = request.Attachments.Select(attachment => new MapPointAttachmentModel
            {
                Name = Path.GetFileName(attachment),
                FileExtension = "jpeg",
                Base64Bytes = Convert.ToBase64String(_imageAttachmentComposer.ComposeScaledImageAttachment(attachment).ToArray())
            }).ToList()
        };
        GetOneResponse<Guid> response;

        try
        {
            response = await _mapObjectClient.AddMapPoint(httpRequest, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok(response.Value);
    }
}
