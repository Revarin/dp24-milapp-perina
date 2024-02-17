using Kris.Client.Common.Enums;

namespace Kris.Client.Core.Services;

public interface IRouterService
{
    Task GoToAsync(string route);
    Task GoToAsync(string route, RouterNavigationType type);
}
