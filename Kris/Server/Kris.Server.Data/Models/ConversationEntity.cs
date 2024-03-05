﻿using Kris.Common.Enums;

namespace Kris.Server.Data.Models;

public sealed class ConversationEntity : EntityBase
{
    public required string Name { get; set; }
    public required ConversationType ConversationType { get; set; }
    // Nullable to break cascade cycle (always required)
    public Guid? SessionId { get; set; }
    public SessionEntity? Session { get; set; }
    public List<SessionUserEntity> Users { get; set; } = new List<SessionUserEntity>();
    public List<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
}
