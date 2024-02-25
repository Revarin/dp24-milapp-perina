namespace Kris.Client.Data.Models.Picker;

public sealed class PositionUploadSettingsItem : IDisplayableItem<int>
{
    public string Display { get; init; }
    public int Value { get; init; }
}
