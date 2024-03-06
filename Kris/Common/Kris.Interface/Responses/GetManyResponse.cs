namespace Kris.Interface.Responses;

public sealed class GetManyResponse<T> : Response
{
    public IEnumerable<T> Values { get; set; }

    public GetManyResponse(IEnumerable<T> values)
    {
        Values = values;
    }
}
