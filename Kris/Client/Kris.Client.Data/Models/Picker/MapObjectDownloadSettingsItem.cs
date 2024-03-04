namespace Kris.Client.Data.Models.Picker;

public sealed class MapObjectDownloadSettingsItem : IDisplayableItem<TimeSpan>
{
    public string Display { get; init; }
    public TimeSpan Value { get; init; }
}
