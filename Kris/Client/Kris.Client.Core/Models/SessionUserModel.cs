using Kris.Common.Enums;

namespace Kris.Client.Core.Models;

public sealed class SessionUserModel
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Nickname { get; set; }
    public UserType UserType { get; set; }
    public DateTime Joined { get; set; }
}
