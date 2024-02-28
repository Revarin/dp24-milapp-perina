namespace Kris.Client.Data.Providers;

public interface ISettingsDataProvider<TSettings>
{
    TSettings GetDefault();
}
