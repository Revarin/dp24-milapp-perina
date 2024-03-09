using Kris.Client.Common.Utility;

namespace Kris.Client.Core.Services;

public interface IRouterService
{
    Task GoToAsync(string route, RouterNavigationType type = RouterNavigationType.PushUpward);
    Task GoToAsync(string route, IDictionary<string, object> parameters, RouterNavigationType type = RouterNavigationType.PushUpward);
    Task GoBackAsync();
}
