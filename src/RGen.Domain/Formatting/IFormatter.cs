using System.Collections;
using System.Collections.Generic;


namespace RGen.Domain.Formatting;


public interface IFormatter
{
	IEnumerable<FormattedRandomValue> Format(IEnumerable randomValues);
}


public interface IFormatter<in T> : IFormatter
{
	IEnumerable<FormattedRandomValue> Format(IEnumerable<T> randomValues);
}