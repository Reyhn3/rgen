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

//TODO: This has to take the format (dec, hex) into consideration
		var (minValue, maxValue) = DetermineMinAndMaxBasedOnLength(lengthOfElement);

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

	internal static (long? minValue, long? maxValue) DetermineMinAndMaxBasedOnLength(int? requestedLength)
	{
		if (!requestedLength.HasValue)
			return (null, null);

		if (requestedLength == 1)
			return (0, 9);

		var minValue = (long)Math.Pow(10, requestedLength.Value - 1);
		var maxValue = (long)Math.Pow(10, requestedLength.Value) - 1;
		return (minValue, maxValue);
	}
}