using Kris.Client.Common.Options;
using Kris.Common;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Kris.Client.Connection.Clients;

public abstract class ClientBase
{
    protected readonly HttpClient _httpClient;
    protected readonly JsonSerializerOptions _serializerOptions;
    protected readonly SettingsOptions _settings;

    private readonly string _controller;

    public ClientBase(IOptions<SettingsOptions> options, string controller)
    {
        _settings = options.Value;
        _controller = controller;

        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        // TODO: Insecure
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri($"{_settings.ApiUrl}/{_controller}");
        _httpClient.DefaultRequestHeaders.Add(Constants.ApiKeyHeader, _settings.ApiKey);
    }

    private HttpClientHandler GetInsecureHandler()
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            if (cert.Issuer.Equals("CN=localhost"))
                return true;
            return errors == System.Net.Security.SslPolicyErrors.None;
        };
        return handler;
    }

    protected async Task<TResult> PostAsync<TData, TResult>(string path, TData data, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(data, _serializerOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(path, content, ct);
        return JsonSerializer.Deserialize<TResult>(response.Content.ReadAsStream(ct), _serializerOptions);
    }
}
