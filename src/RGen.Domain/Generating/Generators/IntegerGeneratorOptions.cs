namespace RGen.Domain.Generating.Generators;


public record struct IntegerGeneratorOptions(
		int? LengthOfElement,
		ulong? Min,
		ulong? Max)
	: IGeneratorOptions;