using System;


namespace RGen.Logic;

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