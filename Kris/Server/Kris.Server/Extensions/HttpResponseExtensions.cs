using Kris.Interface.Responses;

namespace Kris.Server.Extensions;

public static class HttpResponseExtensions
{
    public static Response Ok(this HttpResponse response)
    {
        response.StatusCode = StatusCodes.Status200OK;
        return new Response
        {
            Status = StatusCodes.Status200OK
        };
    }

    public static TResponse? Ok<TResponse>(this HttpResponse response, TResponse data) where TResponse : Response
    {
        response.StatusCode = StatusCodes.Status200OK;
        data.Status = StatusCodes.Status200OK;
        return data;
    }

    public static TResponse? BadRequest<TResponse>(this HttpResponse response, string? message = null) where TResponse : Response
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        response.WriteAsync(message ?? "Bad request").Wait();
        return null;
    }

    public static TResponse? Unauthorized<TResponse>(this HttpResponse response, string? message = null) where TResponse : Response
    {
        response.StatusCode = StatusCodes.Status401Unauthorized;
        response.WriteAsync(message ?? "Unauthorized").Wait();
        return null;
    }

    public static TResponse? NotFound<TResponse>(this HttpResponse response, string? message = null) where TResponse : Response
    {
        response.StatusCode = StatusCodes.Status404NotFound;
        response.WriteAsync(message ?? "Not found").Wait();
        return null;
    }

    public static TResponse? InternalError<TResponse>(this HttpResponse response, string? message = null) where TResponse : Response
    {
        response.StatusCode = StatusCodes.Status500InternalServerError;
        response.WriteAsync(message ?? "Internal error").Wait();
        return null;
    }
}
