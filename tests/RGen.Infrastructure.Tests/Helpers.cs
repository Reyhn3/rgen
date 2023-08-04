using System;
using RGen.Domain.Formatting;


namespace RGen.Infrastructure.Tests;

internal static class Helpers
{
	private const string NullValue = "<null>";

	public static FormatContext? Dump(this FormatContext? item)
	{
		if (item == null)
		{
			Console.WriteLine(NullValue);
			return item;
		}

		Console.WriteLine("Raw:\t\t\t{0}", item.Raw);
		Console.WriteLine("Formatted:\t{0}", item.Formatted);
		return item;
	}


	public static T? Dump<T>(this T? item)
	{
		Console.WriteLine(item?.ToString() ?? NullValue);
		return item;
	}
}