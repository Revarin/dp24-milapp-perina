namespace Kris.Client.Common
{
    public static class Constants
    {
        public static class PreferencesStore
        {
            public static readonly string LastRegionLocation = "LastRegionLocation";
            public static readonly string LastRegionDistance = "LastRegionDistance";
        }

        public static class ConnectionSettings
        {
            public static readonly string GpsInterval = "ConnectionSettingsGpsInterval";
            public static readonly string UserId = "UserSettingsUserId";
            public static readonly string UserName = "UserSettingsUserName";
            public static readonly string UsersLocationInterval = "ConnectionSettingsUsersLocationInterval";
        }

        public static class DefaultSettings
        {
            public static readonly int GpsInterval = 10000;
            public static readonly int UsersLocationInterval = 10000;
        }
    }
}
