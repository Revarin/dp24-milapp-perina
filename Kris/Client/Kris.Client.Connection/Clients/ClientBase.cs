using Kris.Common.Utility;
using Kris.Interface.Responses;
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
        _serializerOptions = JsonSerializerOptionsFactory.CreateHttpJsonSerializerOptions();
    }

    protected async Task<TResult> PostAsync<TData, TResult>(HttpClient httpClient, string path, TData data, CancellationToken ct)
        where TResult : Response, new()
    {
        var requestJson = JsonSerializer.Serialize(data, _serializerOptions);
        var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(path, content, ct);

        TResult responseData;

        try
        {
            responseData = JsonSerializer.Deserialize<TResult>(await response.Content.ReadAsStreamAsync(ct), _serializerOptions);
        }
        catch
        {
            responseData = new TResult()
            {
                Status = (int)response.StatusCode,
                Message = await response.Content.ReadAsStringAsync()
            };
        }

        return responseData;
    }
}
