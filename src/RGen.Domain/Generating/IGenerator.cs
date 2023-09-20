using System.Collections;
using System.Collections.Generic;


namespace RGen.Domain.Generating;


public interface IGenerator
{
//TODO: #42: Refactor to use parameter-object instead of list of parameters
	IEnumerable Generate(
		int numberOfElements,
		int numberOfSets,
		int? lengthOfElement,
		ulong? min,
		ulong? max);
}


public interface IGenerator<out T> : IGenerator
{
//TODO: #42: Refactor to use parameter-object instead of list of parameters
	new IEnumerable<T> Generate(
		int numberOfElements,
		int numberOfSets,
		int? lengthOfElement,
		ulong? min,
		ulong? max);
}