using System;
using Serilog.Core;
using Serilog.Events;
using Spectre.Console;


namespace RGen.Infrastructure.Logging;

public static class LogHelper
{
	public static LoggingLevelSwitch Switch { get; } = new();
	public static bool IsQuiet => Switch.MinimumLevel == LogEventLevel.Fatal;
	public static bool IsNoColorSet => !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("NO_COLOR"));

	public static LogEventLevel ToLogLevel(VerbosityLevel verbosityLevel) =>
		verbosityLevel switch
			{
				VerbosityLevel.Quiet    => LogEventLevel.Fatal,
				VerbosityLevel.Minimal  => LogEventLevel.Warning,
				VerbosityLevel.Normal   => LogEventLevel.Information,
				VerbosityLevel.Detailed => LogEventLevel.Debug,
				VerbosityLevel.Verbose  => LogEventLevel.Verbose,
				_                       => throw new ArgumentOutOfRangeException(nameof(verbosityLevel), verbosityLevel, null)
			};

	//public static bool ShouldSkipLogging(LogLevel level) =>
	//	!ShouldLog(level);

	//public static bool ShouldLog(LogLevel level) =>
	//	Switch.MinimumLevel switch
	//		{
	//			LogEventLevel.Fatal       => false,
	//			LogEventLevel.Error       => level is LogLevel.Critical,
	//			LogEventLevel.Warning     => level is LogLevel.Critical or LogLevel.Warning,
	//			LogEventLevel.Information => level is LogLevel.Critical or LogLevel.Warning or LogLevel.Information,
	//			LogEventLevel.Debug       => level is LogLevel.Critical or LogLevel.Warning or LogLevel.Information or LogLevel.Debug,
	//			LogEventLevel.Verbose     => level is LogLevel.Critical or LogLevel.Warning or LogLevel.Information or LogLevel.Debug or LogLevel.Trace,
	//			_                         => true
	//		};

	public static void PreLog(string color, string message)
	{
		if (IsNoColorSet)
		{
			Console.WriteLine(message);
			return;
		}

		AnsiConsole.MarkupLine($"[{color}]{message}[/]");
	}

	public static void PreLog(Exception ex)
	{
		if (IsNoColorSet)
		{
			Console.WriteLine("ERROR: Unhandled exception");
			Console.WriteLine(ex);
			return;
		}

		AnsiConsole.MarkupLine("[bold Black on Red]ERROR:[/] [Red]Unhandled exception[/]");
		AnsiConsole.WriteException(ex, ExceptionFormats.ShortenTypes | ExceptionFormats.ShortenPaths);
	}

	public static void PrintExceptionDetails(Exception ex)
	{
		if (IsNoColorSet)
		{
			//if (ShouldLog(LogLevel.Debug))
			if (Switch.MinimumLevel is LogEventLevel.Debug or LogEventLevel.Verbose)
				Console.WriteLine(ex);
			//else if (ShouldLog(LogLevel.Critical))
			else if (Switch.MinimumLevel is LogEventLevel.Warning or LogEventLevel.Information)
				Console.WriteLine($"    {ex.Message}");
		}
		else
		{
			//if (ShouldLog(LogLevel.Debug))
			if (Switch.MinimumLevel is LogEventLevel.Debug or LogEventLevel.Verbose)
				AnsiConsole.WriteException(ex, ExceptionFormats.ShortenTypes | ExceptionFormats.ShortenPaths);
			//else if (ShouldLog(LogLevel.Critical))
			else if (Switch.MinimumLevel is LogEventLevel.Warning or LogEventLevel.Information)
				AnsiConsole.MarkupLine($"    [Red]{ex.Message}[/]");
		}
	}
}