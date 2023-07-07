using System;


namespace RGen.Application.Tests;

internal static class Helpers
{
	public static T Dump<T>(this T item)
	{
		Console.WriteLine(item?.ToString() ?? "<null>");
		return item;
	}
}