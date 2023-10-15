namespace Kris.Client.Core
{
    public class NavigationService : INavigationService
    {
        public async Task GoToAsync(string route)
        {
            await Shell.Current.GoToAsync(route);
        }

        public void RegisterRoutes(IEnumerable<Type> pages)
        {
            foreach (var page in pages)
            {
                Routing.RegisterRoute(page.Name, page);
            }
        }
    }
}
