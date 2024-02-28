using Kris.Interface.Models;

namespace Kris.Interface.Requests;

public sealed class StoreUserSettingsRequest : RequestBase
{
    public ConnectionSettingsModel? ConnectionSettings { get; set; }
}
