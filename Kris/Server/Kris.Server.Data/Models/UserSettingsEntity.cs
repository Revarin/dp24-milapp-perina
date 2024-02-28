﻿namespace Kris.Server.Data.Models;

public sealed class UserSettingsEntity : EntityBase
{
    public int? GpsRequestInterval { get; set; }
    public int? PositionUploadFrequency { get; set; }
    public int? PositionDownloadFrequency { get; set; }
    public UserEntity? User { get; set; }

    public bool IsConnectionSettingsNull() => !(GpsRequestInterval.HasValue || PositionUploadFrequency.HasValue || PositionDownloadFrequency.HasValue);
}
