using Kris.Common.Enums;
using Kris.Common.Models;

namespace Kris.Server.Data.Models;

public class SessionUserEntity : EntityBase
{
    public required Guid SessionId { get; set; }
    public SessionEntity? Session { get; set; }
    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    public required UserType UserType { get; set; }
    public required DateTime Joined { get; set; }

    public required string Nickname { get; set; }
    public MapPointSymbol Symbol { get; set; } = new MapPointSymbol
    {
        Shape = MapPointSymbolShape.Circle,
        Color = MapPointSymbolColor.Blue,
        Sign = MapPointSymbolSign.None
    };

    public List<MapPointEntity> MapPoints { get; set; } = new List<MapPointEntity>();
    public List<ConversationEntity> Conversations { get; set; } = new List<ConversationEntity>();
    public List<MessageEntity> SentMessages { get; set; } = new List<MessageEntity>();
}
