namespace Kris.Interface.Responses;

public sealed class GetOneResponse<T> : EmptyResponse
{
    public required T Value { get; set; }
}
