using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;


namespace RGen.Domain.Generating.Generators;

public class IntegerGenerator : IGenerator
{
	public IRandomValues Generate(int n, int o, int? length)
	{
		IEnumerable<IEnumerable<int>> values;

		int? max = null;
		int? min = null;

		if (length.HasValue)
		{
			min = (int)Math.Pow(10, length.Value - 1);
			max = (int)Math.Pow(10, length.Value) - 1;
		}

		if (o > 1)
			values = Set(n, o, min, max);
		else if (n > 1)
			values = new[]
				{
					Multiple(n, min, max)
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