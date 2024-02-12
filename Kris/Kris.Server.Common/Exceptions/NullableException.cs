using System.Runtime.Serialization;

namespace Kris.Server.Common.Exceptions;

[Serializable]
public sealed class NullableException : Exception
{
    public NullableException() : base("Expected value") {}

    public NullableException(string? message) : base(message) {}

    public NullableException(string? message, Exception? innerException) : base(message, innerException) {}

    public NullableException(SerializationInfo info, StreamingContext context) : base(info, context) {}
}
