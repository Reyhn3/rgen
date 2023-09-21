using System.Collections;
using System.Collections.Generic;


namespace RGen.Domain.Generating;


public interface IGenerator
{
	IEnumerable Generate(
		int numberOfElements,
		int numberOfSets,
		object? parameters);
}


public interface IGenerator<out TElement, in TParams> : IGenerator
	where TParams : struct
{
	IEnumerable<TElement> Generate(
		int numberOfElements,
		int numberOfSets,
		TParams parameters);
}