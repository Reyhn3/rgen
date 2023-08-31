using System;
using System.Collections.Generic;
using RGen.Domain.Generating;


namespace RGen.Domain.Tests.Generating;

internal static class GeneratorUtils
{
	public static void PrintSets(IRandomValues values)
	{
		var setIndex = 0;
		foreach (var set in values.ValueSets)
		{
			Console.WriteLine("Set {0}:", setIndex);

			var elementIndex = 0;
			foreach (var element in set)
			{
				Console.WriteLine("  Element {0}:\t{1}", elementIndex, element);
				elementIndex++;
			}

			setIndex++;
		}
	}
}