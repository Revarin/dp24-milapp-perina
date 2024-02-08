using Kris.Common;
using Kris.Server.Common.Options;
using Microsoft.Extensions.Options;

namespace Kris.Server.Middleware;

public sealed class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettingsOptions _settings;

    public ApiKeyMiddleware(RequestDelegate next, IOptions<AppSettingsOptions> settings)
    {
        _next = next;
        _settings = settings.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        string requestKey = context.Request.Headers[Constants.ApiKeyHeader];

        if (requestKey != _settings.ApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }
}
