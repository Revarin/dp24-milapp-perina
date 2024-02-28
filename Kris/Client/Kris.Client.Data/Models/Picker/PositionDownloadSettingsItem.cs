namespace Kris.Client.Data.Models.Picker;

public sealed class PositionDownloadSettingsItem : IDisplayableItem<TimeSpan>
{
    public string Display { get; init; }
    public TimeSpan Value { get; init; }
}
