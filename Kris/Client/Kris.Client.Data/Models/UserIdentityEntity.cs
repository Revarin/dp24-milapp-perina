using Kris.Common.Enums;

namespace Kris.Client.Data.Models;

public class UserIdentityEntity
{
    public Guid UserId {  get; set; }
    public string Login { get; set; }
    public Guid? SessionId { get; set; }
    public string? SessioName { get; set; }
    public UserType? UserType { get; set; }
}
