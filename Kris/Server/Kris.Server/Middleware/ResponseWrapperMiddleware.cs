using Kris.Interface.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json;

namespace Kris.Server.Middleware;

// Source: https://stackoverflow.com/a/60529018
public sealed class ResponseWrapperMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseWrapperMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;
        var originBody = response.Body;

        try
        {
            // To not immediately send response
            // Allows writing to resposne after _next(context)
            var memStream = new MemoryStream();
            response.Body = memStream;

            await _next(context);

            memStream.Position = 0;
            if (response.StatusCode >= StatusCodes.Status400BadRequest)
            {
                using var reader = new StreamReader(memStream);
                var responseBody = reader.ReadToEnd();

                var wrapper = new Response
                {
                    Status = response.StatusCode,
                    Message = responseBody
                };
                var json = JsonSerializer.Serialize(wrapper);

                var memoryStreamModified = new MemoryStream();
                var writer = new StreamWriter(memoryStreamModified);
                writer.Write(json);
                writer.Flush();
                memoryStreamModified.Position = 0;

                await memoryStreamModified.CopyToAsync(originBody);
            }
            else
            {
                await memStream.CopyToAsync(originBody);
            }
        }
        finally
        {
            response.Body = originBody;
        }
    }
}
