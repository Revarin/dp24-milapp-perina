namespace Kris.Client.Data.Models.Picker;

public sealed class GpsIntervalSettingsItem : IDisplayableItem<TimeSpan>
{
    public string Display { get; init; }
    public TimeSpan Value { get; init; }
}
