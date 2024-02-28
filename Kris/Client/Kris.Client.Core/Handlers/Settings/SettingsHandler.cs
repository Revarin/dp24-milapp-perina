using Kris.Client.Core.Mappers;
using Kris.Client.Data.Cache;
using Kris.Interface.Controllers;

namespace Kris.Client.Core.Handlers.Settings;

public abstract class SettingsHandler : BaseHandler
{
    protected readonly IUserController _userClient;
    protected readonly ISettingsStore _settingsStore;
    protected readonly ISettingsMapper _settingsMapper;

    protected SettingsHandler(IUserController userClient, ISettingsStore settingsStore, ISettingsMapper settingsMapper)
    {
        _userClient = userClient;
        _settingsStore = settingsStore;
        _settingsMapper = settingsMapper;
    }
}
