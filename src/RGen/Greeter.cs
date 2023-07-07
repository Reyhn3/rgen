using System;
using System.Linq;
using RGen.Application.Commanding;
using RGen.Properties;


namespace RGen;

internal static class Greeter
{
	public static void Greet(string[] args)
	{
		try
		{
			if (args.Any(a => string.Equals(a, GlobalSilentOption.SilentOption, StringComparison.OrdinalIgnoreCase)))
				return;

			var splashLines = Resources.splash.Split("\r\n");
			Console.ForegroundColor = ConsoleColor.Cyan;
			foreach (var splashLine in splashLines)
				Console.WriteLine(splashLine);

			Console.ResetColor();
		}
		catch
		{
			// ignore
		}
	}
}