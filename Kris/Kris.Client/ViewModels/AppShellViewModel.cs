using System.Windows.Input;
using Kris.Client.Common;

namespace Kris.Client.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ICommand AppearingCommand { get; init; }

        private List<Type> _contentPages = new List<Type>()
        {
            typeof(MapView),
            typeof(MenuView),
            typeof(ConnectionSettingsView),
            typeof(TestView)
        };

        public AppShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            AppearingCommand = new Command(OnAppearing);
        }

        private void OnAppearing()
        {
            _navigationService.RegisterRoutes(_contentPages);
        }
    }
}
