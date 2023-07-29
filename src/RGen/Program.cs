using System;
using System.CommandLine.Parsing;
using System.Text;
using RGen;
using RGen.Application;

Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;

try
{
	var parser = Startup.BuildParser();
	return await parser.InvokeAsync(args);
}
catch (Exception ex)
{
	ConsoleHelper.PrintException(ex, "Unhandled exception");
	return (int)ExitCode.UnhandledException;
}
finally
{
	Console.ResetColor();
}