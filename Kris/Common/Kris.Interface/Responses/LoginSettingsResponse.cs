using Kris.Interface.Models;

namespace Kris.Interface.Responses;

public sealed class LoginSettingsResponse : LoginResponse
{
    public UserSettings Settings { get; set; }

    public sealed class UserSettings
    {
        public ConnectionSettingsModel? ConnectionSettings { get; set; }
    }
}
