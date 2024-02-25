using Kris.Common;
using Kris.Server.Common.Options;
using Microsoft.Extensions.Options;

namespace Kris.Server.Middleware;

public sealed class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly SettingsOptions _settings;

    public ApiKeyMiddleware(RequestDelegate next, IOptions<SettingsOptions> settings)
    {
        _next = next;
        _settings = settings.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestKey = context.Request.Headers[Constants.ApiKeyHeader].FirstOrDefault();

        if (requestKey != _settings.ApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await _next(context);
    }
}
