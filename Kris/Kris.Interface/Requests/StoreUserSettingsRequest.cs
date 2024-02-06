using Kris.Interface.Models;

namespace Kris.Interface.Requests;

public sealed class StoreUserSettingsRequest : RequestBase
{
    public required UserSettings UserSettings { get; set; }
}
