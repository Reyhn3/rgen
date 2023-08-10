using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RGen.Application.Commanding.Explain;
using RGen.Application.Commanding.Globals;
using RGen.Application.Commanding.Integer;
using RGen.Application.Formatting;
using RGen.Application.Writing;
using RGen.Domain;
using RGen.Domain.Generating.Generators;
using RGen.Infrastructure;
using RGen.Infrastructure.Formatting.Console;
using RGen.Infrastructure.Logging;
using RGen.Infrastructure.Writing.Console;
using RGen.Infrastructure.Writing.TextFile;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.SystemConsole.Themes;


namespace RGen;

internal static class Startup
{
	public static Parser BuildParser() =>
		BuildCommandLine()
			.UseDefaults()
			.UseHelp(help =>
				{
					help.HelpBuilder
						.CustomizeLayout(_ =>
							HelpBuilder.Default
								.GetLayout()
								.Skip(1)
								.Prepend(_ =>
									Splash.Render()));
					help.HelpBuilder
						.CustomizeSymbol(
							GlobalVerbosityOption.Verbosity,
							"--verbosity <level>" + Environment.NewLine + "    Quiet, Minimal, Normal, Detailed, Verbose");
				})
			.UseHost(
				Host.CreateDefaultBuilder,
				host => host
					.UseConsoleLifetime()
					.UseContentRoot(AppContext.BaseDirectory)
					.ConfigureLogging(builder => builder
						.ClearProviders()
						.AddSerilog())
					.UseSerilog((_, config) => config
						.WriteTo.Console(
							outputTemplate: "{Level:u1} | {Message:lj}{NewLine}",
							theme: LogHelper.IsNoColorSet ? ConsoleTheme.None : AnsiConsoleTheme.Literate,
							levelSwitch: LogHelper.Switch)
						.Filter.ByExcluding(e => e.Level == LogEventLevel.Fatal)
						.Filter.ByIncludingOnly(Matching.FromSource(typeof(Startup).Namespace!)))
					.ConfigureServices(services => services
						.AddSingleton<IGeneratorService, GeneratorService>()
						.AddSingleton<IntegerGenerator>()
						.AddSingleton(_ =>
							new FormatterFactory()
								.Register<ConsoleFormatterOptions>(o => new ConsoleFormatter(o)))
						.AddSingleton(sp =>
							new WriterFactory()
								.Register<ConsoleWriterOptions>(o => new ConsoleWriter(o))
								.Register<PlainTextFileWriterOptions>(o =>
									new PlainTextFileWriter(sp.GetRequiredService<ILogger<PlainTextFileWriter>>(), o))))
					.UseCommandHandler<GenerateIntegerCommand, GenerateIntegerHandler>()
					.UseCommandHandler<ExplainCommand, ExplainHandler>())
			.AddMiddleware(VerbosityLevelMiddleware.Instance, MiddlewareOrder.Configuration)
			.Build();

	private static CommandLineBuilder BuildCommandLine()
	{
		var rootCommand = new RootCommand(ConsoleHelper.GetProductName(typeof(Program).Assembly))
			{
				new GenerateIntegerCommand(),
				new ExplainCommand()
			};

		rootCommand.AddGlobalOption(GlobalVerbosityOption.Verbosity);
		rootCommand.AddGlobalOption(GlobalVerbosityOption.Quiet);
		rootCommand.AddGlobalOption(GlobalVerbosityOption.Loud);
		rootCommand.AddGlobalOption(GlobalColorOption.Color);
		rootCommand.AddGlobalOption(GlobalOutputOption.Output);

		return new CommandLineBuilder(rootCommand);
	}
}