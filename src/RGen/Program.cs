using System;
using System.CommandLine.Parsing;
using System.Text;
using RGen;
using RGen.Application;

Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
Greeter.Greet(args);

try
{
	var parser = Startup.BuildParser();
	return await parser.InvokeAsync(args);
}
catch (Exception ex)
{
	CliHelper.PrintException(ex, "Unhandled exception");
	return ExitCodes.UnhandledException;
}