using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;


namespace RGen.Domain.Generating.Generators;

public class IntegerGenerator : IGenerator
{
	public IRandomValues Generate(int numberOfElements, int numberOfSets, int? lengthOfElement, int? min, int? max)
	{
		if (numberOfElements < 1)
			throw new ArgumentOutOfRangeException(nameof(numberOfElements), numberOfElements, "The number of elements to generate must be 1 or greater");

		if (numberOfSets < 1)
			throw new ArgumentOutOfRangeException(nameof(numberOfSets), numberOfSets, "The number of sets to generate must be 1 or greater");

		// Int64 has 19 digits, excluding the sign
		if (lengthOfElement is < 1 or > 19)
			throw new ArgumentOutOfRangeException(nameof(lengthOfElement), lengthOfElement, "The length of the generated element must be 1 to 19");

		if (min >= max)
			throw new ArgumentOutOfRangeException(nameof(min), min, "The minimum value is greater than or equal to the maximum value");

//TODO: This has to take the format (dec, hex) into consideration
		var (minValue, maxValue) = DetermineMinAndMax(lengthOfElement, min, max);

		// Check that min/max don't contradict lengthOfElement
		if (min > minValue || maxValue < max)
			throw new InvalidOperationException(
				$"The parameters {nameof(min)} ({min}) and/or {nameof(max)} ({max}) have values with more digits than the parameter {nameof(lengthOfElement)} which is specified to limit results to contain {lengthOfElement} digits");

//TODO: Refactor to support both positive and negative values
//TODO: Refactor to support long
		IEnumerable<IEnumerable<int>> values;

		if (numberOfSets > 1)
			values = Set(numberOfElements, numberOfSets, minValue, maxValue);
		else if (numberOfElements > 1)
			values = new[]
				{
					Multiple(numberOfElements, minValue, maxValue)
				};
		else
			values = new[]
				{
					new[]
						{
							Single(minValue, maxValue)
						}
				};

		return new RandomValues<int>(values);
	}

	private static int Single(long? min, long? max)
	{
//TODO: Refactor to support long
		if (!(min.HasValue && max.HasValue))
			return RandomNumberGenerator.GetInt32(int.MaxValue);

//TODO: Refactor to support long
		return (int)Math.Abs(Math.Floor(new Random().NextDouble() * (max.Value - min.Value + 1d) + (double)min));
	}

	private static IEnumerable<int> Multiple(int n, long? min, long? max) =>
		Enumerable.Range(0, n).Select(_ => Single(min, max));

	private static IEnumerable<IEnumerable<int>> Set(int n, int o, long? min, long? max) =>
		Enumerable.Range(0, o).Select(_ => Multiple(n, min, max));

	internal static (long? minValue, long? maxValue) DetermineMinAndMax(int? lengthOfElement, long? min, long? max)
	{
		if (lengthOfElement == null && min == null && max == null)
			return (null, null);

		if (min >= max)
			throw new ArgumentOutOfRangeException(nameof(min), min, "The minimum value is greater than or equal to the maximum value");

		if (!lengthOfElement.HasValue)
			return (min, max);

		int? requiredLengthForMin = min.HasValue ? MathUtils.CountNumberOfDecimalDigits(min.Value) : null;
		int? requiredLengthForMax = max.HasValue ? MathUtils.CountNumberOfDecimalDigits(max.Value) : null;

		// Verify that min/max doesn't conflict with the length
		if (requiredLengthForMin > lengthOfElement)
			throw new InvalidOperationException(
				$"The specified maximum digit length {lengthOfElement} is less than the required length for the specified minimum value {requiredLengthForMin}");

		if (requiredLengthForMax > lengthOfElement)
			throw new InvalidOperationException(
				$"The specified maximum digit length {lengthOfElement} is less than the required length for the specified maximum value {requiredLengthForMax}");

		if (requiredLengthForMin < lengthOfElement)
			throw new InvalidOperationException(
				$"The specified maximum digit length {lengthOfElement} is less than the required length for the specified maximum value {requiredLengthForMin}");

		if (requiredLengthForMax < lengthOfElement)
			throw new InvalidOperationException(
				$"The specified maximum digit length {lengthOfElement} is greater than the required length for the specified maximum value {requiredLengthForMax}");

		var minValue = min ?? (lengthOfElement == 1 ? 0 : (long)Math.Pow(10, lengthOfElement.Value - 1));
		var maxValue = max ?? (lengthOfElement == 1 ? 9 : (long)Math.Pow(10, lengthOfElement.Value) - 1);
		return (minValue, maxValue);
	}
}