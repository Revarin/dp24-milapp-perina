namespace Kris.Interface.Responses;

public sealed class GetManyResponse<T> : Response
{
    public required IEnumerable<T> Values { get; set; }
}
