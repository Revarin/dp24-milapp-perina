namespace Kris.Interface.Responses;

public class Response<T> : Response where T : class
{
    public T? Body { get; set; }
}

public class Response
{
    public required int Status { get; set; }
    public IEnumerable<string> Messages { set; get; } = new List<string>();
}
