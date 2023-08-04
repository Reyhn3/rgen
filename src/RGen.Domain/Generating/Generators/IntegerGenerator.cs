using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;


namespace RGen.Domain.Generating.Generators;

public class IntegerGenerator : IGenerator
{
	public IRandomValues Generate(int n, int o)
	{
		IEnumerable<IEnumerable<int>> values;

		if (o > 1)
			values = Set(n, o);
		else if (n > 1)
			values = new[]
				{
					Multiple(n)
				};
		else
			values = new[]
				{
					new[]
						{
							Single()
						}
				};

		return new RandomValues<int>(values);
	}

	private static int Single() =>
		RandomNumberGenerator.GetInt32(int.MaxValue);

	private static IEnumerable<int> Multiple(int n) =>
		Enumerable.Range(0, n).Select(_ => Single());

	private static IEnumerable<IEnumerable<int>> Set(int n, int o) =>
		Enumerable.Range(0, o).Select(_ => Multiple(n));
}