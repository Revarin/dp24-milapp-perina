namespace Kris.Client.Common
{
    public static class Constants
    {
        public static class PreferencesStore
        {
            public static readonly string LastRegionKey = "LastRegionKey";

            public static readonly string MapSpanLocationKey = "Location";
            public static readonly string MapSpanDistanceKey = "Distance";

            public static readonly string SettingsGpsInterval = "SettingsGpsInterval";
        }

        public static class DefaultSettings
        {
            public static readonly int GpsInterval = 10000;
        }
    }
}
