using System;
using RGen.Domain.Rendering;


namespace RGen.Infrastructure.Tests;

internal static class Helpers
{
	private const string NullValue = "<null>";

	public static RenderContext? Dump(this RenderContext? item)
	{
		if (item == null)
		{
			Console.WriteLine(NullValue);
			return item;
		}

		Console.WriteLine("Formatted:\t\t\t{0}", item.Formatted);
		Console.WriteLine("Rendered:\t{0}", item.Rendered);
		return item;
	}


	public static T? Dump<T>(this T? item)
	{
		Console.WriteLine(item?.ToString() ?? NullValue);
		return item;
	}
}