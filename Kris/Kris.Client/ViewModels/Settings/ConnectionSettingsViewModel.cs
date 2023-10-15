using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using CommunityToolkit.Maui.Core.Extensions;
using Kris.Client.Common;
using Kris.Client.Core;
using Kris.Client.Data;

namespace Kris.Client.ViewModels
{
    public class ConnectionSettingsViewModel : ViewModelBase
    {
        private readonly AppSettings _settings;
        private readonly IMessageService _messageService;
        private readonly IPreferencesStore _preferencesStore;
        private readonly ISessionFacade _sessionFacade;

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetPropertyValue(ref _userName, value); }
        }
        private ObservableCollection<GpsIntervalItem> _gpsIntervalItems;
        public ObservableCollection<GpsIntervalItem> GpsIntervalItems
        {
            get { return _gpsIntervalItems; }
            set { SetPropertyValue(ref _gpsIntervalItems, value); }
        }
        private GpsIntervalItem _gpsIntervalSelectedItem;
        public GpsIntervalItem GpsIntervalSelectedItem
        {
            get { return _gpsIntervalSelectedItem; }
            set { SetPropertyValue(ref _gpsIntervalSelectedItem, value); }
        }
        private ObservableCollection<UsersLocationIntervalItem> _usersLocationIntervalItems;
        public ObservableCollection<UsersLocationIntervalItem> UsersLocationIntervalItems
        {
            get { return _usersLocationIntervalItems; }
            set { SetPropertyValue(ref _usersLocationIntervalItems, value); }
        }
        private UsersLocationIntervalItem _usersLocationIntervalSelectedItem;
        public UsersLocationIntervalItem UsersLocationIntervalSelectedItem
        {
            get { return _usersLocationIntervalSelectedItem; }
            set { SetPropertyValue(ref _usersLocationIntervalSelectedItem, value); }
        }

        public ICommand UserNameCompletedCommand { get; init; }
        public ICommand GpsIntervalSelectedIndexChangedCommand { get; init; }
        public ICommand UsersLocationIntervalSelectedIndexChangedCommand { get; init; }

        private ConnectionSettings _connectionSettings;

        public ConnectionSettingsViewModel(IMessageService messageService, IPreferencesStore preferencesStore, ISessionFacade sessionFacade,
            IDataSource<GpsIntervalItem> gpsItervalDataSource, IDataSource<UsersLocationIntervalItem> usersLocationIntervalDataSource,
            IConfiguration config)
        {
            _messageService = messageService;
            _preferencesStore = preferencesStore;
            _sessionFacade = sessionFacade;
            _settings = config.GetRequiredSection("Settings").Get<AppSettings>();

            UserNameCompletedCommand = new Command(OnUserNameCompletedAsync);
            GpsIntervalSelectedIndexChangedCommand = new Command(OnGpsIntervalSelectedIndexChanged);
            UsersLocationIntervalSelectedIndexChangedCommand = new Command(OnUsersLocationIntervalSelectedIndexChanged);

            _connectionSettings = _preferencesStore.GetConnectionSettings();

            UserName = _connectionSettings.UserName;
            GpsIntervalItems = gpsItervalDataSource.Get().ToObservableCollection();
            GpsIntervalSelectedItem = GpsIntervalItems.Single(p => p.Value == _connectionSettings.GpsInterval);
            UsersLocationIntervalItems = usersLocationIntervalDataSource.Get().ToObservableCollection();
            UsersLocationIntervalSelectedItem = UsersLocationIntervalItems.Single(p => p.Value == _connectionSettings.UsersLocationInterval);
        }

        private async void OnUserNameCompletedAsync()
        {
            var newName = UserName;

            if (_settings.ServerEnabled)
            {
                if (_connectionSettings.UserId > 0)
                {
                    await _sessionFacade.UpdateUserAsync(_connectionSettings.UserId, newName);
                }
                else
                {
                    var user = await _sessionFacade.CreateUserAsync(newName);
                    _connectionSettings.UserId = user.Id;
                }

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

        private void OnUsersLocationIntervalSelectedIndexChanged()
        {
            var newInterval = UsersLocationIntervalSelectedItem;

            _connectionSettings.UsersLocationInterval = newInterval.Value;
            _preferencesStore.SetConnectionSettings(_connectionSettings);

            _messageService.Send(new ConnectionSettingsChangedMessage
            {
                UsersLocationIntervalChanged = true,
                Settings = _connectionSettings
            });
        }
    }
}
