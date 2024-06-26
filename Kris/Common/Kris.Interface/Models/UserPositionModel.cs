﻿using Kris.Common.Models;

namespace Kris.Interface.Models;

public sealed class UserPositionModel
{
    public required Guid UserId { get; set; }
    public string? UserName { get; set; }
    public required DateTime Updated { get; set; }
    public required List<GeoSpatialPosition> Positions { get; set; }
    public required MapPointSymbol Symbol { get; set; }
}
