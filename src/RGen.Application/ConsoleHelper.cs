using System;
using System.Diagnostics;
using System.Reflection;


namespace RGen.Application;

public static class ConsoleHelper
{
	public static void PrintException(Exception ex, string label)
	{
		Console.ForegroundColor = ConsoleColor.DarkRed;
		Console.WriteLine("{0}: {1}", label, ex.Message);
		Console.ResetColor();
	}

	public static void SetConsoleTitle(Assembly assembly)
	{
		try
		{
			var name = GetProductName(assembly);
			var version = FileVersionInfo.GetVersionInfo(assembly.Location)?.ProductVersion ?? assembly.GetName()?.Version?.ToString();
			Console.Title = $@"{name} v{version} (PID {Environment.ProcessId})";
		}
		catch
		{
			// ignore: running as service
		}
	}

	public static string GetProductName(Assembly assembly)
	{
		const string fallback = "AES Crypt Tool";

		try
		{
			return assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? fallback;
		}
		catch
		{
			return fallback;
		}
	}
}