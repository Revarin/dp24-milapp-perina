using Kris.Client.Common.Utility;

namespace Kris.Client.Core.Services;

public sealed class RouterService : IRouterService
{
    public async Task GoToAsync(string route, string type = RouterNavigationType.PushUpward)
    {
        await Shell.Current.GoToAsync($"{type}{route}");
    }
}
