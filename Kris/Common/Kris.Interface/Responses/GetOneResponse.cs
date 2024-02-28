namespace Kris.Interface.Responses;

public sealed class GetOneResponse<T> : Response
{
    public T Value { get; set; }
}
