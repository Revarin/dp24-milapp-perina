using Kris.Common.Enums;

namespace Kris.Interface.Models;

public sealed class SessionUserModel
{
    public required Guid Id { get; set; }
    public required string Login {  get; set; }
    public required string Nickname { get; set; }
    public required UserType UserType { get; set; }
    public required DateTime Joined { get; set; }
}
