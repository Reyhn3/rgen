using System.Collections;
using System.Collections.Generic;


namespace RGen.Domain.Formatting;


public interface IFormatter
{
	IEnumerable<FormattedRandomValue> Format(IEnumerable randomValues, object? parameters);
}


public interface IFormatter<in TElement, in TParams> : IFormatter
	where TParams : struct
{
	IEnumerable<FormattedRandomValue> Format(IEnumerable<TElement> randomValues, TParams parameters);
}