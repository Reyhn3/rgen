using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;


namespace RGen.Domain.Generators;

public class IntegerGenerator : IIntegerGenerator
{
	private static int Single() =>
		RandomNumberGenerator.GetInt32(int.MaxValue);

	public IEnumerable<int> Multiple(int n) =>
		Enumerable.Range(0, n).Select(_ => Single());

	public IEnumerable<IEnumerable<int>> Set(int n, int o) =>
		Enumerable.Range(0, o).Select(_ => Multiple(n));
}