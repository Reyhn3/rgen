using System;
using System.Linq;
using RGen.Properties;


namespace RGen;

internal static class Greeter
{
	public static void Greet()
	{
		try
		{
//TODO: Stay quiet if silent option is requested

			var splashLines = Resources.splash.Split("\r\n");
			var maxWidth = splashLines.Max(l => l.Length);
			//TODO: Calculate the buffer offset.

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