using System;


namespace RGen.Application;

//TODO: Replace with Spectre
public static class CliHelper
{
	public static void PrintException(Exception ex, string label)
	{
		Console.ForegroundColor = ConsoleColor.DarkRed;
		Console.WriteLine("{0}: {1}", label, ex.Message);
		Console.ResetColor();
	}
}