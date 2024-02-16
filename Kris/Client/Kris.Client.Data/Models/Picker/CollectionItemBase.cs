namespace Kris.Client.Data
{
    public abstract class CollectionItemBase<T>
    {
        public string Display { get; set; }
        public T Value { get; set; }
    }
}
