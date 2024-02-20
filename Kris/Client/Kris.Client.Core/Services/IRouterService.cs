using Kris.Client.Common.Utility;

namespace Kris.Client.Core.Services;

public interface IRouterService
{
    Task GoToAsync(string route, string type = RouterNavigationType.PushUpward);
}
