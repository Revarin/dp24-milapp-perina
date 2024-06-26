﻿namespace Kris.Server.Common.Exceptions;

[Serializable]
public class MappingException : Exception
{
	public MappingException() { }

	public MappingException(string message) : base(message) { }

	public MappingException(string message, Exception inner) : base(message, inner) { }

	protected MappingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		: base(info, context) { }
}
