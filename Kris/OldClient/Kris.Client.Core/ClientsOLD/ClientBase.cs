using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Kris.Client.Common;

namespace Kris.Client.Core
{
    public abstract class ClientBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly SettingsOptions _settings;

        private readonly string _controller;

        public ClientBase(IConfiguration config, string controller)
        {
            _settings = config.GetRequiredSection("Settings").Get<SettingsOptions>();
            _controller = controller;

            // TODO: Insecure
            _httpClient = new HttpClient(GetInsecureHandler());
        }

        protected StringContent GetRequestContent<T>(T body)
        {
            return new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }

        protected Uri GetUri(string path)
        {
            return new Uri($"{_settings.ServerUrl}/{_controller}/{path}");
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
    }
}
