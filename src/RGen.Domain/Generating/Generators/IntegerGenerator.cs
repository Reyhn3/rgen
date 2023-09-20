using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;


namespace RGen.Domain.Generating.Generators;


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


public class IntegerGenerator : Generator<ulong>
{
//TODO: Extract base class and move checks, Set, Multiple etc. up.
	public override IEnumerable<ulong> Generate(int numberOfElements, int numberOfSets, int? lengthOfElement, ulong? min, ulong? max)
	{
//TODO: Consider limit the max number of elements (in each set)
		if (numberOfElements < 1)
			throw new ArgumentOutOfRangeException(nameof(numberOfElements), numberOfElements, "The number of elements to generate must be 1 or greater");

//TODO: Consider limit the max number of sets
		if (numberOfSets < 1)
			throw new ArgumentOutOfRangeException(nameof(numberOfSets), numberOfSets, "The number of sets to generate must be 1 or greater");

		// Int64 has 19 digits, excluding the sign
		if (lengthOfElement is < 1 or > 19)
			throw new ArgumentOutOfRangeException(nameof(lengthOfElement), lengthOfElement, "The length of the generated element must be 1 to 19");

		if (min >= max)
			throw new ArgumentOutOfRangeException(nameof(min), min, "The minimum value is greater than or equal to the maximum value");

//TODO: #19: This has to take the format (dec, hex) into consideration
		var (minValue, maxValue) = DetermineMinAndMax(lengthOfElement, min, max);

		// Check that min/max don't contradict lengthOfElement
		if (min > minValue || maxValue < max)
			throw new InvalidOperationException(
				$"The parameters {nameof(min)} ({min}) and/or {nameof(max)} ({max}) have values with more digits than the parameter {nameof(lengthOfElement)} which is specified to limit results to contain {lengthOfElement} digits");

//TODO: #34: Refactor to support both positive and negative values
		var values = Set(numberOfElements, numberOfSets, minValue, maxValue);
		return values;
	}

//TODO: #39: Consider doing this more efficient, by e.g. generating a bigger byte array and create numbers from subsets of it
	private static IEnumerable<ulong> Set(int numberOfElements, int numberOfSets, ulong min, ulong max) => Enumerable
		.Range(0, numberOfSets * numberOfElements)
		.Select(x => Single(min, max));

	internal static ulong Single(ulong min, ulong max)
	{
//TODO: #38: Consider using Intel's rdrand instruction set (see https://github.com/JebteK/RdRand)
//TODO: Make upper boundary inclusive

		// Use simple generation if limited to Int32
		if (max <= int.MaxValue)
			return (ulong)RandomNumberGenerator.GetInt32((int)Math.Max(0, min), (int)max);

		return GenerateInt64InRange(min, max);


		ulong GenerateInt64InRange(ulong umin, ulong umax)
		{
			// Inspired by StackOverflow: https://stackoverflow.com/a/13095144/68955

			const int maxRetries = 10;
			var retries = 0;
			var range = umax - umin;
			ulong proposed;

			do
			{
				if (retries++ > maxRetries)
					throw new Exception("Retry count reached when generating clamped integer");

				proposed = (ulong)BitConverter.ToInt64(RandomNumberGenerator.GetBytes(8));
			} while (proposed > ulong.MaxValue - (ulong.MaxValue % range + 1) % range);

			return proposed % range + umin;
		}
	}

	internal static (ulong minValue, ulong maxValue) DetermineMinAndMax(int? lengthOfElement, ulong? min, ulong? max)
	{
//TODO: #34: Refactor to support both positive and negative values
		if (lengthOfElement == null && min == null && max == null)
			return (ulong.MinValue, ulong.MaxValue);

		if (min >= max)
			throw new ArgumentOutOfRangeException(nameof(min), min, "The minimum value is greater than or equal to the maximum value");

		if (!lengthOfElement.HasValue)
			return (min ?? ulong.MinValue, max ?? ulong.MaxValue);

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

		var minValue = min ?? (lengthOfElement == 1 ? 0 : (ulong)Math.Pow(10, lengthOfElement.Value - 1));
		var maxValue = max ?? (lengthOfElement == 1 ? 9 : (ulong)Math.Pow(10, lengthOfElement.Value) - 1);
		return (minValue, maxValue);
	}
}