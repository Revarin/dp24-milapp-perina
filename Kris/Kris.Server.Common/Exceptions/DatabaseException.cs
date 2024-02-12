using System.Data.Common;

namespace Kris.Server.Common.Exceptions;

[Serializable]
public class DatabaseException : DbException
{
    public DatabaseException() {}

    public DatabaseException(string? message) : base(message) {}

    public DatabaseException(string? message, Exception? innerException) : base(message, innerException) {}

    protected DatabaseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
    : base(info, context) { }
}
