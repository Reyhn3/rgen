using System;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RGen.Application;
using RGen.Infrastructure.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.SystemConsole.Themes;


namespace RGen;


internal static class Startup
{
	public static Parser BuildParser() => CLI
		.CreateCommandLine()
		.UseHost(
			Host.CreateDefaultBuilder,
			host => host
				.UseConsoleLifetime()
				.UseContentRoot(AppContext.BaseDirectory)
				.ConfigureLogging(builder => builder
					.ClearProviders()
					.AddSerilog())
				.UseSerilog((_, config) => config
					.MinimumLevel.ControlledBy(LogHelper.Switch)
					.WriteTo.Console(
						outputTemplate: "{Level:u1} | {Message:lj}{NewLine}",
						theme: LogHelper.IsNoColorSet ? ConsoleTheme.None : AnsiConsoleTheme.Literate,
						levelSwitch: LogHelper.Switch)
					.Filter.ByExcluding(e => e.Level == LogEventLevel.Fatal)
					.Filter.ByIncludingOnly(Matching.FromSource(typeof(Startup).Namespace!)))
				.ConfigureServices(services => services
					.AddRgen())
				.UseCommandLine())
		.ConfigureCommandLine() // IMPORTANT: This needs to be LAST (or it won't resolve services)
		.Build();
}