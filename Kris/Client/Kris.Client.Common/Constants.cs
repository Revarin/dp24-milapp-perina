namespace Kris.Client.Common
{
    public static class Constants
    {
        public static class PreferencesStore
        {
            public static readonly string LastRegionLocationKey = "LastRegionLocation";
            public static readonly string LastRegionDistanceKey = "LastRegionDistance";
        }

        public static class ConnectionSettings
        {
            public static readonly string GpsIntervalKey = "ConnectionSettingsGpsInterval";
            public static readonly string UserIdKey = "ConnectionSettingsUserId";
            public static readonly string UserNameKey = "ConnectionSettingsUserName";
            public static readonly string UsersLocationIntervalKey = "ConnectionSettingsUsersLocationInterval";

            public static readonly int DefaultGpsInterval = 10000;
            public static readonly int DefaultUsersLocationInterval = 10000;
        }
    }
}
