using System.Reflection;
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
            
            var response = await _httpClient.PostAsync(MethodBase.GetCurrentMethod().Name, content);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<CreateUserResponse>(await response.Content.ReadAsStringAsync());
        }

        public async Task UpdateUserName(UpdateUserNameRequest request)
        {
            var content = GetRequestContent(request);

            var response = await _httpClient.PutAsync(MethodBase.GetCurrentMethod().Name, content);

            response.EnsureSuccessStatusCode();
        }
    }
}
