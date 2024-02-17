﻿using Kris.Interface.Responses;

namespace Kris.Server.Extensions;

public static class HttpResponseExtensions
{
    public static Response<T> Ok<T>(this HttpResponse response, T? data = null) where T : EmptyResponse
    {
        response.StatusCode = StatusCodes.Status200OK;
        return new Response<T>
        {
            Status = response.StatusCode,
            Body = data
        };
    }

    public static Response<T> BadRequest<T>(this HttpResponse response) where T : EmptyResponse
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        return new Response<T>
        {
            Status = response.StatusCode,
            Messages = new List<string> { "Bad request" },
            Body = null
        };
    }

    public static Response<T> BadRequest<T>(this HttpResponse response, IEnumerable<string> messages) where T : EmptyResponse
    {
        response.StatusCode = StatusCodes.Status400BadRequest;
        return new Response<T>
        {
            Status = response.StatusCode,
            Messages = messages,
            Body = null
        };
    }

    public static Response<T> Unauthorized<T>(this HttpResponse response) where T : EmptyResponse
    {
        response.StatusCode = StatusCodes.Status401Unauthorized;
        return new Response<T>
        {
            Status = response.StatusCode,
            Body = null
        };
    }

    public static Response<T> Unauthorized<T>(this HttpResponse response, IEnumerable<string> messages) where T : EmptyResponse
    {
        response.StatusCode = StatusCodes.Status401Unauthorized;
        return new Response<T>
        {
            Status = response.StatusCode,
            Messages = messages,
            Body = null
        };
    }

    public static Response<T> NotFound<T>(this HttpResponse response) where T : EmptyResponse
    {
        response.StatusCode = StatusCodes.Status404NotFound;
        return new Response<T>
        {
            Status = response.StatusCode,
            Body = null
        };
    }

    public static Response<T> NotFound<T>(this HttpResponse response, IEnumerable<string> messages) where T : EmptyResponse
    {
        response.StatusCode = StatusCodes.Status404NotFound;
        return new Response<T>
        {
            Status = response.StatusCode,
            Messages = messages,
            Body = null
        };
    }
}
