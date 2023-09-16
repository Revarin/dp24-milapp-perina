using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Extensions;
using Kris.Client.Common;
using Kris.Client.Data;

namespace Kris.Client.ViewModels
{
    public class ConnectionSettingsViewModel : ViewModelBase
    {
        private readonly IPreferencesStore _preferencesDataStore;
        private readonly IGpsService _gpsService;

        private GpsIntervalItem _gpsIntervalselectedItem;
        public GpsIntervalItem GpsIntervalSelectedItem
        {
            get { return _gpsIntervalselectedItem; }
            set { SetPropertyValue(ref _gpsIntervalselectedItem, value); }
        }
        private ObservableCollection<GpsIntervalItem> _gpsIntervalitems;
        public ObservableCollection<GpsIntervalItem> GpsIntervalItems
        {
            get { return _gpsIntervalitems; }
            set { SetPropertyValue(ref _gpsIntervalitems, value); }
        }

        public ICommand SelectedIndexChangedCommand { get; init; }

        public ConnectionSettingsViewModel(IPreferencesStore preferencesStore, IDataSource<GpsIntervalItem> gpsItervalDataSource, IGpsService gpsService)
        {
            _preferencesDataStore = preferencesStore;
            _gpsService = gpsService;

            SelectedIndexChangedCommand = new Command(OnSelectedIndexChanged);

            var currentGpsInterval = _preferencesDataStore.Get(Constants.PreferencesStore.SettingsGpsInterval, Constants.DefaultSettings.GpsInterval);

            GpsIntervalItems = gpsItervalDataSource.Get().ToObservableCollection();
            GpsIntervalSelectedItem = GpsIntervalItems.Single(p => p.Value == currentGpsInterval);
        }

        private async void OnSelectedIndexChanged()
        {
            var current = GpsIntervalSelectedItem;

            // DEBUG
            var toast = Toast.Make($"{current.Display}: {current.Value}");
            await toast.Show();

            _preferencesDataStore.Set(Constants.PreferencesStore.SettingsGpsInterval, current.Value);
            _gpsService.SetupListener(current.Value, current.Value);
        }
    }
}
