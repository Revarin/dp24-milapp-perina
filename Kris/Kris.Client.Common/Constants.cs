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

        public static class UserSettings
        {
            public static readonly string UserId = "UserSettingsUserId";
            public static readonly string UserName = "UserSettingsUserName";
        }

        public static class ConnectionSettings
        {
            public static readonly string GpsInterval = "ConnectionSettingsGpsInterval";
        }

        public static class DefaultSettings
        {
            public static readonly int GpsInterval = 10000;
        }
    }
}
