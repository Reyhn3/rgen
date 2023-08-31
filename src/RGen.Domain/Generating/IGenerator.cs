namespace RGen.Domain.Generating;

public interface IGenerator
{
//TODO: Refactor to use parameter-object instead of list of parameters
	IRandomValues Generate(
		int numberOfElements,
		int numberOfSets,
		int? lengthOfElement,
		int? min,
		int? max);
}