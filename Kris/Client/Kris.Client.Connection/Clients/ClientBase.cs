using Kris.Client.Data.Cache;
using Kris.Common.Utility;
using Kris.Interface.Responses;
using System.Text;
using System.Text.Json;

namespace Kris.Client.Connection.Clients;

public abstract class ClientBase
{
    private const string ContentTypeJson = "application/json";

    protected readonly IIdentityStore _identityStore;
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly string _controller;
    protected readonly JsonSerializerOptions _serializerOptions;

    public ClientBase(IIdentityStore identityStore, IHttpClientFactory httpClientFactory, string controller)
    {
        _identityStore = identityStore;
        _httpClientFactory = httpClientFactory;

        _controller = controller;
        _serializerOptions = JsonSerializerOptionsFactory.CreateHttpJsonSerializerOptions();
    }

    protected async Task<TResult> PostAsync<TData, TResult>(HttpClient httpClient, string path, TData data, CancellationToken ct)
        where TResult : Response, new()
    {
        var requestJson = JsonSerializer.Serialize(data, _serializerOptions);
        var content = new StringContent(requestJson, Encoding.UTF8, ContentTypeJson);
        var response = await httpClient.PostAsync(path, content, ct);

        return await ParseResponse<TResult>(response, ct);
    }

    protected async Task<TResult> PutAsync<TResult>(HttpClient httpClient, string path, CancellationToken ct)
        where TResult : Response, new()
    {
        var response = await httpClient.PutAsync(path, null);

        return await ParseResponse<TResult>(response, ct);
    }

    protected async Task<TResult> PutAsync<TData, TResult>(HttpClient httpClient, string path, TData data, CancellationToken ct)
        where TResult : Response, new()
    {
        var requestJson = JsonSerializer.Serialize(data, _serializerOptions);
        var content = new StringContent(requestJson, Encoding.UTF8, ContentTypeJson);
        var response = await httpClient.PutAsync(path, content, ct);

        return await ParseResponse<TResult>(response, ct);
    }

    protected async Task<TResult> GetAsync<TResult>(HttpClient httpClient, string path, CancellationToken ct)
        where TResult : Response, new()
    {
        var response = await httpClient.GetAsync(path);

        return await ParseResponse<TResult>(response, ct);
    }

    private async Task<TResult> ParseResponse<TResult>(HttpResponseMessage response, CancellationToken ct)
        where TResult : Response, new()
    {
        TResult responseData;

        try
        {
            responseData = JsonSerializer.Deserialize<TResult>(await response.Content.ReadAsStreamAsync(ct), _serializerOptions);
        }
        catch
        {
            responseData = new TResult()
            {
                Status = 500,
                Message = await response.Content.ReadAsStringAsync()
            };
        }

        return responseData;
    }
}
