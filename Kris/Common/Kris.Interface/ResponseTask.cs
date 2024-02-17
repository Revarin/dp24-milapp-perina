using Kris.Interface.Responses;
using System.Runtime.CompilerServices;

namespace Kris.Interface;

public class ResponseTask<T> : Task<T> where T : EmptyResponse
{
    public T? Body { get; set; }
    public required int ResponseStatus { get; set; }
    public IEnumerable<string> Messages { set; get; } = new List<string>();

    public ResponseTask(Func<T> function) : base(function)
    {
    }

    public ResponseTask(Func<object?, T> function, object? state) : base(function, state)
    {
    }

    public ResponseTask(Func<T> function, CancellationToken cancellationToken) : base(function, cancellationToken)
    {
    }

    public ResponseTask(Func<T> function, TaskCreationOptions creationOptions) : base(function, creationOptions)
    {
    }

    public ResponseTask(Func<object?, T> function, object? state, CancellationToken cancellationToken) : base(function, state, cancellationToken)
    {
    }

    public ResponseTask(Func<object?, T> function, object? state, TaskCreationOptions creationOptions) : base(function, state, creationOptions)
    {
    }

    public ResponseTask(Func<T> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, cancellationToken, creationOptions)
    {
    }

    public ResponseTask(Func<object?, T> function, object? state, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, state, cancellationToken, creationOptions)
    {
    }

    public new ResponseTaskAwaiter GetAwaiter() => default(ResponseTaskAwaiter);
}

public struct ResponseTaskAwaiter : INotifyCompletion
{
    public void GetResult() { }

    public bool IsCompleted => true;

    public void OnCompleted(Action continuation) { }
}
