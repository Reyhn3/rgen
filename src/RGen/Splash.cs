using System;
using RGen.Properties;


namespace RGen;

internal static class Splash
{
	public static void Render()
	{
		try
		{
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