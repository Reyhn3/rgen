using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;


namespace RGen.Domain.Generating.Generators;

public class IntegerGenerator : IGenerator
{
	public IRandomValues Generate(int numberOfElements, int numberOfSets, int? lengthOfElement)
	{
		if (numberOfElements < 1)
			throw new ArgumentOutOfRangeException(nameof(numberOfElements), numberOfElements, "The number of elements to generate must be 1 or greater");

		if (numberOfSets < 1)
			throw new ArgumentOutOfRangeException(nameof(numberOfSets), numberOfSets, "The number of sets to generate must be 1 or greater");

		if (lengthOfElement is < 1)
			throw new ArgumentOutOfRangeException(nameof(lengthOfElement), lengthOfElement, "The length of the generated element must be 1 or greater");

		IEnumerable<IEnumerable<int>> values;

		int? max = null;
		int? min = null;

		if (lengthOfElement.HasValue)
		{
			min = (int)Math.Pow(10, lengthOfElement.Value - 1);
			max = (int)Math.Pow(10, lengthOfElement.Value) - 1;
		}

		if (numberOfSets > 1)
			values = Set(numberOfElements, numberOfSets, min, max);
		else if (numberOfElements > 1)
			values = new[]
				{
					Multiple(numberOfElements, min, max)
				};
		else
			values = new[]
				{
					new[]
						{
							Single(min, max)
						}
				};

		return new RandomValues<int>(values);
	}

	private static int Single(int? min, int? max)
	{
		if (!(min.HasValue && max.HasValue))
			return RandomNumberGenerator.GetInt32(int.MaxValue);

		return (int)Math.Abs(Math.Floor(new Random().NextDouble() * (max.Value - min.Value + 1d) + (double)min));
	}

	private static IEnumerable<int> Multiple(int n, int? min, int? max) =>
		Enumerable.Range(0, n).Select(_ => Single(min, max));

	private static IEnumerable<IEnumerable<int>> Set(int n, int o, int? min, int? max) =>
		Enumerable.Range(0, o).Select(_ => Multiple(n, min, max));
}