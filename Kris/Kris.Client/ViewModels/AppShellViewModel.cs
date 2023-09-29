using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kris.Client.Core;

namespace Kris.Client.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        private readonly IInitializationManager _initializationManager;

        public ICommand AppearingCommand { get; init; }

        private List<Type> _contentPages = new List<Type>()
        {
            typeof(MapView),
            typeof(MenuView),
            typeof(ConnectionSettingsView),
            typeof(TestView)
        };

        public AppShellViewModel(IInitializationManager initializationManager)
        {
            _initializationManager = initializationManager;

            AppearingCommand = new Command(OnAppearing);
        }

        private async void OnAppearing()
        {
            _initializationManager.InitializeNavigation(_contentPages);

            await _initializationManager.InitializePermissionsAsync();

            await _initializationManager.InitialiteUserDataAsync();

            WeakReferenceMessenger.Default.Send(new AppInitializedMessage(null));
        }
    }
}
