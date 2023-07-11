using System;


namespace RGen.Application;

public static class ConsoleHelper
{
	public static void PrintException(Exception ex, string label)
	{
		Console.ForegroundColor = ConsoleColor.DarkRed;
		Console.WriteLine("{0}: {1}", label, ex.Message);
		Console.ResetColor();
	}
}