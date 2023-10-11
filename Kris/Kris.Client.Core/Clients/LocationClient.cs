using Microsoft.Extensions.Configuration;
using Kris.Interface;
using System.Reflection;
using Newtonsoft.Json;

namespace Kris.Client.Core
{
    public class LocationClient : ClientBase, ILocationController
    {
        public LocationClient(IConfiguration config) : base(config, "Location")
        {
        }

        public async Task<LoadUsersLocationsResponse> LoadUsersLocations(LoadUsersLocationsRequest request)
        {
            var content = GetRequestContent(request);

            var uri = GetUri("LoadUsersLocations");
            var response = await _httpClient.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<LoadUsersLocationsResponse>(await response.Content.ReadAsStringAsync());
        }

        public async Task SaveUserLocation(SaveUserLocationRequest request)
        {
            var content = GetRequestContent(request);

            var uri = GetUri("SaveUserLocation");
            var response = await _httpClient.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();
        }
    }
}
