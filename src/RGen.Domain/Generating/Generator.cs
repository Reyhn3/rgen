using System.Collections;
using System.Collections.Generic;


namespace RGen.Domain.Generating;


public abstract class Generator<T> : IGenerator<T>
{
	IEnumerable IGenerator.Generate(
		int numberOfElements,
		int numberOfSets,
		int? lengthOfElement,
		ulong? min,
		ulong? max) =>
		Generate(
			numberOfElements,
			numberOfSets,
			lengthOfElement,
			min,
			max);

	public abstract IEnumerable<T> Generate(
		int numberOfElements,
		int numberOfSets,
		int? lengthOfElement,
		ulong? min,
		ulong? max);
}