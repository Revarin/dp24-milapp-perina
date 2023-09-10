namespace Kris.Client.Common
{
    public interface INavigationService
    {
        Task GoToAsync(string route);
        void RegisterRoutes(IEnumerable<Type> pages);
    }
}
