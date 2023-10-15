namespace Kris.Client.Core
{
    public interface INavigationService
    {
        Task GoToAsync(string route);
        void RegisterRoutes(IEnumerable<Type> pages);
    }
}
