﻿using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.Maui.Maps;
using CommunityToolkit.Maui.Alerts;
using Kris.Client.Behaviors;
using Kris.Client.Common;
using Kris.Client.Data;
using Kris.Client.Core;

namespace Kris.Client.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;
        private readonly IPermissionsService _permissionsService;
        private readonly IPreferencesStore _preferencesStore;
        private readonly IGpsService _gpsService;

        private MapSpan _currentRegion;
        public MapSpan CurrentRegion
        {
            get { return _currentRegion; }
            set { SetPropertyValue(ref _currentRegion, value); }
        }
        public ObservableCollection<MapPin> PinsSource { get; } = new();
        public MoveToRegionRequest MoveToRegion { get; init; } = new MoveToRegionRequest();

        public ICommand LoadedCommand { get; init; }
        public ICommand MoveToUserCommand { get; init; }

        private ConnectionSettings _connectionSettings;

        public MapViewModel(IMessageService messageService, IPermissionsService permissionsService, IPreferencesStore preferencesStore, IGpsService gpsService)
        {
            _messageService = messageService;
            _permissionsService = permissionsService;
            _preferencesStore = preferencesStore;
            _gpsService = gpsService;

            CurrentRegion = new MapSpan(new Location(), 10, 10);
            LoadedCommand = new Command(OnLoaded);
            MoveToUserCommand = new Command(OnMoveToUser);

            _messageService.Register<ShellInitializedMessage>(this, OnShellInitialized);

            _connectionSettings = _preferencesStore.GetConnectionSettings();
        }

        private void OnLoaded()
        {
            var lastRegion = _preferencesStore.GetLastRegion();
            if (lastRegion != null)
            {
                MoveToRegion.Execute(lastRegion);
            }
        }

        private async void OnShellInitialized(object sender, ShellInitializedMessage _)
        {
            var locationPermission = await _permissionsService.CheckPermissionAsync<Permissions.LocationWhenInUse>();
            if (locationPermission.HasFlag(PermissionStatus.Granted) && await _gpsService.IsGpsEnabled(2))
            {
                _gpsService.RaiseGpsLocationEvent += OnGpsNewLocation;
                if (!_gpsService.IsListening)
                {
                    _gpsService.SetupListener(_connectionSettings.GpsInterval, _connectionSettings.GpsInterval);
                    await _gpsService.StartListeningAsync();
                }
            }
            else
            {
                var gpsUnavailableToast = Toast.Make($"GPS service is not available.");
                await gpsUnavailableToast.Show();
            }
        }

        private void OnMoveToUser()
        {
            var user = PinsSource.SingleOrDefault(p => p.PinType == CustomPinType.User);
            if (user != null)
            {
                var userRegion = MapSpan.FromCenterAndRadius(user.Location, CurrentRegion.Radius);
                MoveToRegion.Execute(userRegion);
            }
        }

        private async void OnGpsNewLocation(object sender, GpsLocationEventArgs e)
        {
#if DEBUG
            var t = Toast.Make($"LAT:{e.Location.Latitude} LONG:{e.Location.Longitude} ALT:{e.Location.Altitude}");
            await t.Show();
#endif

            var user = PinsSource.SingleOrDefault(p => p.PinType == CustomPinType.User);
            if (user != null)
            {
                PinsSource.Remove(user);
            }
            PinsSource.Add(new MapPin
            {
                Location = e.Location,
                PinType = CustomPinType.User,
                ImageSource = ImageSource.FromFile("user.png"),
                Name = "User",
            });
        }
    }
}
