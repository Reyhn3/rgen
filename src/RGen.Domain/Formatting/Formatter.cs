using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace RGen.Domain.Formatting;


public abstract class Formatter<TElement, TParams> : IFormatter<TElement, TParams>
	where TParams : struct
{
	IEnumerable<FormattedRandomValue> IFormatter.Format(IEnumerable randomValues, object? parameters) =>
		Format(randomValues.Cast<TElement>(), (TParams)(parameters ?? default(TParams)));

	public abstract IEnumerable<FormattedRandomValue> Format(IEnumerable<TElement> randomValues, TParams parameters);
}