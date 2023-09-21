namespace RGen.Domain.Generating.Generators;


public record struct IntegerParameters(
	int? LengthOfElement,
	ulong? Min,
	ulong? Max);