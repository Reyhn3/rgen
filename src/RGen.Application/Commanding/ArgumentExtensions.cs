using System.CommandLine;


namespace RGen.Application.Commanding;

internal static class ArgumentExtensions
{
	public static Argument<int> InValidRangeOnly(this Argument<int> arg, int lowerBoundaryInclusive, int upperBoundaryInclusive)
	{
		arg.AddValidator(result => Validate.Range(result, lowerBoundaryInclusive, upperBoundaryInclusive));
		return arg;
	}

	public static Option<int> InValidRangeOnly(this Option<int> arg, int lowerBoundaryInclusive, int upperBoundaryInclusive)
	{
		arg.AddValidator(result => Validate.Range(result, lowerBoundaryInclusive, upperBoundaryInclusive));
		return arg;
	}
}