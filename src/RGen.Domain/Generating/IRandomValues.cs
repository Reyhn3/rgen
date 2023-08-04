using System.Collections.Generic;


namespace RGen.Domain.Generating;

public interface IRandomValues
{
	IEnumerable<IEnumerable<object>> ValueSets { get; }
}


public interface IRandomValues<out T> : IRandomValues
{
	new IEnumerable<IEnumerable<T>> ValueSets { get; }
}