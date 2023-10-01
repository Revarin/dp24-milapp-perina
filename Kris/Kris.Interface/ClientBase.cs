using System.Text.Json;

namespace Kris.Interface
{
    public abstract class ClientBase
    {
        protected HttpClient _httpClient;
        protected JsonSerializerOptions _serializerOptions;

        public ClientBase()
        {
            _httpClient = new HttpClient();
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };
        }

    }
}
