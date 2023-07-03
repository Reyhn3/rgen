using System;
using System.Collections.Generic;
using System.Linq;


namespace RGen.Logic.Integer;

public class IntegerGenerator : IIntegerGenerator
{
	private static int Single() =>
		DateTime.Now.Microsecond;

	public IEnumerable<int> Multiple(int n) =>
		Enumerable.Range(0, n).Select(_ => Single());

	public IEnumerable<IEnumerable<int>> Set(int n, int o) =>
		Enumerable.Range(0, o).Select(_ => Multiple(n));
}