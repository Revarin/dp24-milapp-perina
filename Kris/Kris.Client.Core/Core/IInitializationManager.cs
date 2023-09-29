namespace Kris.Client.Core
{
    public interface IInitializationManager
    {
        void InitializeNavigation(IEnumerable<Type> pages);
        Task InitializePermissionsAsync();
        Task InitialiteUserDataAsync();
    }
}
