using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Kris.Client.Common;
using Newtonsoft.Json;
using System.Text;

namespace Kris.Client.Core
{
    public abstract class ClientBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly AppSettings _settings;

        public ClientBase(IConfiguration config, string controllerName = null)
        {
            _settings = config.GetRequiredSection("Settings").Get<AppSettings>();

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = string.IsNullOrEmpty(controllerName) ?
                new Uri(_settings.ServerUrl) :
                new Uri($"{_settings.ServerUrl}/{controllerName}");
        }

        protected StringContent GetRequestContent<T>(T body)
        {
            return new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }
    }
}
