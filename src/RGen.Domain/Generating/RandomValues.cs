using System.Collections.Generic;
using System.Linq;


namespace RGen.Domain.Generating;

public abstract class RandomValues : IRandomValues
{
	protected RandomValues(IEnumerable<IEnumerable<object>> valueSets)
	{
		ValueSets = valueSets;
	}

	public IEnumerable<IEnumerable<object>> ValueSets { get; }

	public static IRandomValues<T> Create<T>(IEnumerable<IEnumerable<T>> randomValues) =>
		new RandomValues<T>(randomValues);
}


public class RandomValues<T> : RandomValues, IRandomValues<T>
{
	public RandomValues(IEnumerable<IEnumerable<T>> valueSets)
		: base(ChangeType(valueSets))
	{}

	public new IEnumerable<IEnumerable<T>> ValueSets =>
		(IEnumerable<IEnumerable<T>>)base.ValueSets;

	private static IEnumerable<IEnumerable<object>> ChangeType(IEnumerable<IEnumerable<T>> original) =>
		original.Where(s => s != null).Select(YieldNestedEnumerable);

	private static IEnumerable<object> YieldNestedEnumerable(IEnumerable<T> set) =>
		set.Where(element => element != null).Cast<object>();
}