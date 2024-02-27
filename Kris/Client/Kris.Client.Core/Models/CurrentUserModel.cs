using Kris.Common.Enums;

namespace Kris.Client.Core.Models;

public sealed class CurrentUserModel
{
    public Guid Id { get; set; }
    public string Login {  get; set; }
    public Guid? SessionId { get; set; }
    public UserType? UserType { get; set; }
    public DateTime LoginExpiration { get; set; }
}
