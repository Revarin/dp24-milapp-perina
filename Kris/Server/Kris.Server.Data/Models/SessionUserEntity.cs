using Kris.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kris.Server.Data.Models;

public class SessionUserEntity : EntityBase
{
    [NotMapped]
    [Obsolete(null, true)]
    public new Guid Id { get; set; }
    public required Guid SessionId { get; set; }
    public SessionEntity? Session { get; set; }
    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    public required UserType UserType { get; set; }
    public required DateTime Joined { get; set; }
}
