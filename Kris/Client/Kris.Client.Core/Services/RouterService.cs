using Kris.Client.Common.Enums;

namespace Kris.Client.Core.Services;

public sealed class RouterService : IRouterService
{
    public async Task GoToAsync(string route)
    {
        await Shell.Current.GoToAsync($"///{route}");
    }

    public async Task GoToAsync(string route, RouterNavigationType type)
    {
        string prefix;
        switch (type)
        {
            case RouterNavigationType.PushUpward:
                prefix = "";
                break;
            case RouterNavigationType.PushDownward:
                prefix = "/";
                break;
            case RouterNavigationType.ReplaceUpward:
                prefix = "//";
                break;
            case RouterNavigationType.ReplaceDownward:
                prefix = "///";
                break;
            default:
                prefix = "///";
                break;
        }

        await Shell.Current.GoToAsync($"{prefix}{route}");
    }
}
