using Kris.Client.Common.Options;
using Kris.Common;
using Microsoft.Extensions.Options;

namespace Kris.Client.Connection;

public sealed class HttpClientFactory : IHttpClientFactory
{
    private readonly ConnectionOptions _settings;

    public HttpClientFactory(IOptions<ConnectionOptions> settings)
    {
        _settings = settings.Value;
    }

    public HttpClient CreateHttpClient(string controller)
    {
        HttpClient client;

#if DEBUG
        client = new HttpClient(GetPlatformMessageHandler());
#else
        client = new HttpClient();
#endif

        client.BaseAddress = new Uri($"{_settings.ApiUrl}/api/{controller}/");
        client.DefaultRequestHeaders.Add(Constants.ApiKeyHeader, _settings.ApiKey);

        return client;
    }

    public HttpClient CreateAuthentizedHttpClient(string controller)
    {
        throw new NotImplementedException();
    }

    // Source: https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/local-web-services?view=net-maui-7.0#local-web-services-running-over-https
    private HttpMessageHandler GetPlatformMessageHandler()
    {
#if ANDROID
        var handler = new Xamarin.Android.Net.AndroidMessageHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            return true;
        };
        return handler;
#elif IOS
        var handler = new NSUrlSessionHandler
        {
            TrustOverrideForUrl = IsHttpsLocalhost
        };
        return handler;
#else
        throw new PlatformNotSupportedException("Only Android and iOS supported.");
#endif
    }

#if IOS
    private bool IsHttpsLocalhost(NSUrlSessionHandler sender, string url, Security.SecTrust trust)
    {
        return true;
    }
#endif

}
