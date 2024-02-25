namespace Kris.Client.Data.Models;

public interface IDisplayableItem<T>
{
    public string Display { get; init; }
    public T Value { get; init; }
}
