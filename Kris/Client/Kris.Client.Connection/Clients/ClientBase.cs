using System.Text;
using System.Text.Json;

namespace Kris.Client.Connection.Clients;

public abstract class ClientBase
{
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly string _controller;
    protected readonly JsonSerializerOptions _serializerOptions;

    public ClientBase(IHttpClientFactory httpClientFactory, string controller)
    {
        _httpClientFactory = httpClientFactory;

        _controller = controller;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    protected async Task<TResult> PostAsync<TData, TResult>(HttpClient httpClient, string path, TData data, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(data, _serializerOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(path, content, ct);
        return JsonSerializer.Deserialize<TResult>(response.Content.ReadAsStream(ct), _serializerOptions);
    }
}
