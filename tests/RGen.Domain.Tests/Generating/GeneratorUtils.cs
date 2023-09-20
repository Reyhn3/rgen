using System;
using System.Collections.Generic;


namespace RGen.Domain.Tests.Generating;


internal static class GeneratorUtils
{
	public static void PrintSets<T>(IEnumerable<T> values)
	{
		var elementIndex = 0;
		foreach (var element in values)
		{
			Console.WriteLine("Element {0}:\t{1}", elementIndex, element);
			elementIndex++;
		}
	}
}