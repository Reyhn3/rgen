using System;
using RGen.Infrastructure.Logging;
using RGen.Properties;


namespace RGen;

internal static class Splash
{
	public static void Render()
	{
		try
		{
			var splashLines = Resources.splash.Split("\r\n");

			if (!LogHelper.IsNoColorSet)
				Console.ForegroundColor = ConsoleColor.Cyan;

			foreach (var splashLine in splashLines)
				Console.WriteLine(splashLine);

			if (!LogHelper.IsNoColorSet)
				Console.ResetColor();
		}
		catch
		{
			// ignore
		}
	}
}