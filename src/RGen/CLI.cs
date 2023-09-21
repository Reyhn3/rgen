using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.Linq;
using Microsoft.Extensions.Hosting;
using RGen.Application.Commanding.Explain;
using RGen.Application.Commanding.Globals;
using RGen.Application.Commanding.Integer;
using RGen.Application.Commanding.Middlewares;
using RGen.Infrastructure;
using RGen.Infrastructure.Logging;


namespace RGen;


public static class CLI
{
	public static CommandLineBuilder CreateCommandLine()
	{
		var rootCommand = new RootCommand(ConsoleHelper.GetProductName(typeof(Program).Assembly))
			{
				new GenerateIntegerCommand(),
				new ExplainCommand()
			};

		rootCommand.AddGlobalOption(GlobalOutputOption.Output);

		rootCommand.AddGlobalOption(GlobalVerbosityOption.Verbosity);
		rootCommand.AddGlobalOption(GlobalVerbosityOption.Quiet);
		rootCommand.AddGlobalOption(GlobalVerbosityOption.Loud);
		rootCommand.AddGlobalOption(GlobalColorOption.Color);

		return new CommandLineBuilder(rootCommand);
	}

	public static IHostBuilder UseCommandLine(this IHostBuilder host) => host
		.UseCommandHandler<GenerateIntegerCommand, GenerateIntegerHandler>()
		.UseCommandHandler<ExplainCommand, ExplainHandler>();

	public static CommandLineBuilder ConfigureCommandLine(this CommandLineBuilder builder) => builder
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
						"--verbosity <level>" + Environment.NewLine + "    " + string.Join(", ", Enum.GetValues<VerbosityLevel>().OrderBy(v => (int)v)));
				help.HelpBuilder
					.CustomizeSymbol(
						GlobalVerbosityOption.Quiet,
						"  -q");
				help.HelpBuilder
					.CustomizeSymbol(
						GlobalVerbosityOption.Loud,
						"  -v");
			})
		.AddMiddleware(VerbosityLevelMiddleware.Instance, MiddlewareOrder.Configuration)
		.AddMiddleware(CommandTranscriber.Instance);
}