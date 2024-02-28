namespace Kris.Interface.Responses;

public sealed class GetManyResponse<T> : Response
{
    public IEnumerable<T> Values { get; set; }
}
