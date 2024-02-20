namespace Kris.Interface.Responses;

public sealed class GetOneResponse<T> : Response
{
    public required T Value { get; set; }
}
