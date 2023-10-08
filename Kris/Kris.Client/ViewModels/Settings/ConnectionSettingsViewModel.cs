using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Core.Extensions;
using Kris.Client.Core;
using Kris.Client.Data;

namespace Kris.Client.ViewModels
{
    public class ConnectionSettingsViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;
        private readonly IPreferencesStore _preferencesStore;
        private readonly ISessionFacade _sessionFacade;

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetPropertyValue(ref _userName, value); }
        }
        private ObservableCollection<GpsIntervalItem> _gpsIntervalitems;
        public ObservableCollection<GpsIntervalItem> GpsIntervalItems
        {
            get { return _gpsIntervalitems; }
            set { SetPropertyValue(ref _gpsIntervalitems, value); }
        }
        private GpsIntervalItem _gpsIntervalselectedItem;
        public GpsIntervalItem GpsIntervalSelectedItem
        {
            get { return _gpsIntervalselectedItem; }
            set { SetPropertyValue(ref _gpsIntervalselectedItem, value); }
        }

        public ICommand UserNameCompletedCommand { get; init; }
        public ICommand GpsIntervalSelectedIndexChangedCommand { get; init; }

        private ConnectionSettings _connectionSettings;

        public ConnectionSettingsViewModel(IMessageService messageService, IPreferencesStore preferencesStore,
            IDataSource<GpsIntervalItem> gpsItervalDataSource, ISessionFacade sessionFacade)
        {
            _messageService = messageService;
            _preferencesStore = preferencesStore;
            _sessionFacade = sessionFacade;

            UserNameCompletedCommand = new Command(OnUserNameCompletedAsync);
            GpsIntervalSelectedIndexChangedCommand = new Command(OnGpsIntervalSelectedIndexChanged);

            _connectionSettings = _preferencesStore.GetConnectionSettings();

            UserName = _connectionSettings.UserName;
            GpsIntervalItems = gpsItervalDataSource.Get().ToObservableCollection();
            GpsIntervalSelectedItem = GpsIntervalItems.Single(p => p.Value == _connectionSettings.GpsInterval);
        }

        private async void OnUserNameCompletedAsync()
        {
            var newName = UserName;

            if (_connectionSettings.UserId > 0)
            {
                await _sessionFacade.UpdateUserAsync(_connectionSettings.UserId, newName);
            }
            else
            {
                var user = await _sessionFacade.CreateUserAsync(newName);
                _connectionSettings.UserId = user.Id;
            }

            _connectionSettings.UserName = newName;
            _preferencesStore.SetConnectionSettings(_connectionSettings);

            _messageService.Send(new ConnectionSettingsChangedMessage
            {
                UserNameChanged = true,
                Settings = _connectionSettings
            });
        }

        private void OnGpsIntervalSelectedIndexChanged()
        {
            var newInterval = GpsIntervalSelectedItem;

            _connectionSettings.GpsInterval = newInterval.Value;
            _preferencesStore.SetConnectionSettings(_connectionSettings);

            _messageService.Send(new ConnectionSettingsChangedMessage
            {
                GpsIntervalChanged = true,
                Settings = _connectionSettings
            });
        }
    }
}
