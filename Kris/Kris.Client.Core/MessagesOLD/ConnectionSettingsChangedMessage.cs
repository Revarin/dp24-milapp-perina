namespace Kris.Client.Core
{
    public class ConnectionSettingsChangedMessage : MessageBase
    {
        public bool UserNameChanged { get; set; }
        public bool GpsIntervalChanged { get; set; }
        public bool UsersLocationIntervalChanged { get; set; }
        public ConnectionSettings Settings { get; set; }
    }
}
