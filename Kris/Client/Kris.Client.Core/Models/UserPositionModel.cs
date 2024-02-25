﻿namespace Kris.Client.Core.Models;

public sealed class UserPositionModel
{
    public required Guid UserId { get; set; }
    public string? UserName { get; set; }
    public required DateTime Updated { get; set; }
    public required List<Location> Positions { get; set; }
}