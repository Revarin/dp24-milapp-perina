namespace Kris.Client.Utility;

public interface IViewRequest<T>
{
    void Execute(T data);
}
