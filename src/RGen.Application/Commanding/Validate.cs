using System.CommandLine.Parsing;


namespace RGen.Application.Commanding;

internal static class Validate
{
	public static void Range(OptionResult result, int lowerBoundaryInclusive, int upperBoundaryInclusive) =>
		Range((SymbolResult)result, lowerBoundaryInclusive, upperBoundaryInclusive);

	public static void Range(ArgumentResult result, int lowerBoundaryInclusive, int upperBoundaryInclusive) =>
		Range((SymbolResult)result, lowerBoundaryInclusive, upperBoundaryInclusive);

	public static void Range(SymbolResult result, int lowerBoundaryInclusive, int upperBoundaryInclusive)
	{
		foreach (var token in result.Tokens)
		{
			if (!int.TryParse(token.Value, out var value))
				result.ErrorMessage = $"Token value '{token.Value}' could not be parsed as an int";

			if (lowerBoundaryInclusive <= value && value <= upperBoundaryInclusive)
				continue;

			result.ErrorMessage = $"Value must be equal to or within {lowerBoundaryInclusive:N0} to {upperBoundaryInclusive:N0}";
		}
	}
}