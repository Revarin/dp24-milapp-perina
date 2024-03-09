using Kris.Client.Common.Events;
using Kris.Client.Data.Models;

namespace Kris.Client.Core.Listeners;

public interface IBackgroundHandler
{
    public event EventHandler<ResultEventArgs> ErrorOccured;

    Task ExecuteAsync(ConnectionSettingsEntity connectionSettings, UserIdentityEntity userIdentity, uint iteration, CancellationToken ct);
}
