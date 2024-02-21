using Kris.Client.Common.Utility;

namespace Kris.Client.Core.Services;

public sealed class RouterService : IRouterService
{
    public async Task GoToAsync(string route, RouterNavigationType type = RouterNavigationType.PushUpward)
    {
        var prefix = "";

        switch (type)
        {
            case RouterNavigationType.PushDownward: prefix = "/"; break;
            case RouterNavigationType.ReplaceUpward: prefix = "//"; break;
            case RouterNavigationType.ReplaceDownward: prefix = "///"; break;
            default: break;
        }

        await Shell.Current.GoToAsync($"{prefix}{route}");
    }
}
