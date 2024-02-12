using Kris.Server.Core.Models;

namespace Kris.Server.Core.Requests;

public abstract class AuthentizedRequest
{
    public required CurrentUserModel User { get; set; }
}
