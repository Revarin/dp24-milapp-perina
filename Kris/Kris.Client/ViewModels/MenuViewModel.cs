using System.Windows.Input;
using Kris.Client.Core;

namespace Kris.Client.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ICommand ConnectionSettingsClickedCommand { get; init; }
        
        public MenuViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            ConnectionSettingsClickedCommand = new Command(OnConnectionSettingsClicked);
        }

        private async void OnConnectionSettingsClicked()
        {
            await _navigationService.GoToAsync(nameof(ConnectionSettingsView));
        }
    }
}
