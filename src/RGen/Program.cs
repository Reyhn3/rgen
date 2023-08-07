using System;
using System.CommandLine.Parsing;
using System.Text;
using System.Threading;
using RGen;
using RGen.Application;
using RGen.Infrastructure;
using Spectre.Console;

Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
ConsoleHelper.SetConsoleTitle(typeof(Program).Assembly);

try
{
	using (new Mutex(false, "rgen", out var isFirstInstance))
	{
		if (!isFirstInstance)
		{
			AnsiConsole.MarkupLine("[Red]Another instance of the program is already running.[/]");
			return (int)ExitCode.MultipleInstances;
		}

		var parser = Startup.BuildParser();
		return await parser.InvokeAsync(args);
	}
}
catch (Exception ex)
{
	AnsiConsole.MarkupLine("[bold Black on Red]ERROR:[/] [Red]Unhandled exception[/]");
	AnsiConsole.WriteException(ex, ExceptionFormats.ShortenTypes | ExceptionFormats.ShortenPaths);
	return (int)ExitCode.UnhandledException;
}
finally
{
	Console.ResetColor();
}