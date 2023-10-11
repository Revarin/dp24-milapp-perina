using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Kris.Interface;

namespace Kris.Client.Core
{
    public class SessionClient : ClientBase, ISessionController
    {
        public SessionClient(IConfiguration config) : base(config, "Session")
        {
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            var content = GetRequestContent(request);

            var uri = GetUri("CreateUser");
            var response = await _httpClient.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<CreateUserResponse>(await response.Content.ReadAsStringAsync());
        }

        public async Task UpdateUserName(UpdateUserNameRequest request)
        {
            var content = GetRequestContent(request);

            var uri = GetUri("UpdateUserName");
            var response = await _httpClient.PutAsync(uri, content);

            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> UserExists(UserExistsRequest request)
        {
            var content = GetRequestContent(request);

            var uri = GetUri("UserExists");
            var response = await _httpClient.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
        }
    }
}
