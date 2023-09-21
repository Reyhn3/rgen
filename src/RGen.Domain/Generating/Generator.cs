using System.Collections;
using System.Collections.Generic;


namespace RGen.Domain.Generating;


public abstract class Generator<TElement, TParams> : IGenerator<TElement, TParams>
	where TParams : struct
{
	IEnumerable IGenerator.Generate(
		int numberOfElements,
		int numberOfSets,
		object? parameters) =>
		Generate(
			numberOfElements,
			numberOfSets,
			(TParams)(parameters ?? default(TParams)));

	public abstract IEnumerable<TElement> Generate(
		int numberOfElements,
		int numberOfSets,
		TParams parameters);
}