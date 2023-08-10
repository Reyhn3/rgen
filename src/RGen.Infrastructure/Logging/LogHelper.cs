using System;
using Serilog.Core;
using Serilog.Events;


namespace RGen.Infrastructure.Logging;

public static class LogHelper
{
	public static LoggingLevelSwitch Switch { get; } = new();
	public static bool IsQuiet => Switch.MinimumLevel == LogEventLevel.Fatal;

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

	public static bool ShouldSkipLogging(LogLevel level) =>
		!ShouldLog(level);

	public static bool ShouldLog(LogLevel level) =>
		Switch.MinimumLevel switch
			{
				LogEventLevel.Fatal       => false,
				LogEventLevel.Error       => level is LogLevel.Critical,
				LogEventLevel.Warning     => level is LogLevel.Critical or LogLevel.Warning,
				LogEventLevel.Information => level is LogLevel.Critical or LogLevel.Warning or LogLevel.Information,
				LogEventLevel.Debug       => level is LogLevel.Critical or LogLevel.Warning or LogLevel.Information or LogLevel.Debug,
				LogEventLevel.Verbose     => level is LogLevel.Critical or LogLevel.Warning or LogLevel.Information or LogLevel.Debug or LogLevel.Trace,
				_                         => true
			};

	public static void PrintExceptionDetails(Exception ex)
	{
		if (ShouldLog(LogLevel.Debug))
			AnsiConsole.WriteException(ex, ExceptionFormats.ShortenTypes | ExceptionFormats.ShortenPaths);
		else if (ShouldLog(LogLevel.Critical))
			AnsiConsole.MarkupLine($"    [Red]{ex.Message}[/]");
	}
}