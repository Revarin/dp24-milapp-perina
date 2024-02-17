namespace Kris.Interface.Responses;

public sealed class GetManyResponse<T> : EmptyResponse
{
    public required IEnumerable<T> Values { get; set; }
}
