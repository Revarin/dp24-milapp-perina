using Kris.Client.Common.Utility;

namespace Kris.Client.Core.Services;

public sealed class RouterService : IRouterService
{
    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public async Task GoToAsync(string route, RouterNavigationType type = RouterNavigationType.PushUpward)
    {
        var prefix = GetPrefix(type);
        await Shell.Current.GoToAsync($"{prefix}{route}");
    }

    public async Task GoToAsync(string route, IDictionary<string, object> parameters, RouterNavigationType type = RouterNavigationType.PushUpward)
    {
        var prefix = GetPrefix(type);
        await Shell.Current.GoToAsync($"{prefix}{route}", parameters);
    }

    private string GetPrefix(RouterNavigationType type)
    {
        var prefix = "";

        switch (type)
        {
            case RouterNavigationType.PushDownward: prefix = "/"; break;
            case RouterNavigationType.ReplaceUpward: prefix = "//"; break;
            case RouterNavigationType.ReplaceDownward: prefix = "///"; break;
            default: break;
        }

        return prefix;
    }
}
