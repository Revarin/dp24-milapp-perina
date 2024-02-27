using Kris.Interface.Models;

namespace Kris.Interface.Responses;

public sealed class LoginResponse : IdentityResponse
{
    public UserSettings Settings { get; set; }

    public sealed class UserSettings
    {
        public ConnectionSettingsModel? ConnectionSettings { get; set; }
    }
}
