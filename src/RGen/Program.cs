using System;
using System.CommandLine.Parsing;
using System.Text;
using System.Threading;
using RGen;
using RGen.Application;
using RGen.Infrastructure;
using RGen.Infrastructure.Logging;

Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
ConsoleHelper.SetConsoleTitle(typeof(Program).Assembly);

try
{
	using (new Mutex(false, "rgen", out var isFirstInstance))
	{
		if (!isFirstInstance)
		{
			LogHelper.PreLog("Red", "Another instance of the program is already running.");
			return (int)ExitCode.MultipleInstances;
		}

		var parser = Startup.BuildParser();
		return await parser.InvokeAsync(args);
	}
}
catch (Exception ex)
{
	LogHelper.PreLog(ex);
	return (int)ExitCode.UnhandledException;
}
finally
{
	Console.ResetColor();
}