﻿namespace Kris.Client.Common
{
    public static class Constants
    {
        public static class PreferencesStore
        {
            public static readonly string LastRegionLocation = "LastRegionLocation";
            public static readonly string LastRegionDistance = "LastRegionDistance";
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
