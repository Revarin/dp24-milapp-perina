using FluentResults;
using Kris.Client.Common.Errors;
using Kris.Client.Core.Requests;
using Kris.Interface.Controllers;
using Kris.Interface.Responses;
using MediatR;
using System.Net;

namespace Kris.Client.Core.Handlers.MapObjects;

public sealed class DeleteMapPointCommandHandler : MapObjectsHandler, IRequestHandler<DeleteMapPointCommand, Result>
{
    public DeleteMapPointCommandHandler(IMapObjectController mapObjectClient)
        : base(mapObjectClient)
    {
    }

    public async Task<Result> Handle(DeleteMapPointCommand request, CancellationToken cancellationToken)
    {
        Response response;
        
        try
        {
            response = await _mapObjectClient.DeleteMapPoint(request.Id, cancellationToken);
        }
        catch (WebException)
        {
            return Result.Fail(new ConnectionError());
        }

        if (!response.IsSuccess())
        {
            if (response.IsUnauthorized()) return Result.Fail(new UnauthorizedError());
            else if (response.IsNotFound()) return Result.Fail(new EntityNotFoundError());
            else return Result.Fail(new ServerError(response.Message));
        }

        return Result.Ok();
    }
}
