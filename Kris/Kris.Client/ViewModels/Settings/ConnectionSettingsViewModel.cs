using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Extensions;
using Kris.Client.Data;

namespace Kris.Client.ViewModels
{
    public class ConnectionSettingsViewModel : ViewModelBase
    {
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

        public ConnectionSettingsViewModel(IDataSource<GpsIntervalItem> gpsItervalDataSource)
        {
            GpsIntervalItems = gpsItervalDataSource.Get().ToObservableCollection();
            GpsIntervalSelectedItem = GpsIntervalItems.Single(p => p.Value == 10000);

            SelectedIndexChangedCommand = new Command(OnSelectedIndexChanged);
        }

        private void OnSelectedIndexChanged()
        {
            var toast = Toast.Make($"{GpsIntervalSelectedItem.Display}: {GpsIntervalSelectedItem.Value}");
            toast.Show();
        }
    }
}
