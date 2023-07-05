using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RGen.Logic;
using RGen.Logic.Integer;
using RGen.Logic.Output;


namespace RGen;

internal static class Startup
{
	public static Parser BuildParser() =>
		BuildCommandLine()
			.UseDefaults()
			.UseHost(
				Host.CreateDefaultBuilder,
				host => host
					.ConfigureLogging(builder => builder.ClearProviders())
					.UseConsoleLifetime()
					.UseContentRoot(AppContext.BaseDirectory)
					.ConfigureServices(services => services
						.AddSingleton<IIntegerGenerator, IntegerGenerator>()
						.AddSingleton<IOutput, ConsoleOutput>())
					.UseCommandHandler<GenerateIntegerCommand, GenerateIntegerHandler>())
			.Build();

	private static CommandLineBuilder BuildCommandLine()
	{
		var rootCommand = new RootCommand("Generate random values")
			{
				new GenerateIntegerCommand()
			};
		rootCommand.AddGlobalOption(GlobalSilentOption.Create());
		return new CommandLineBuilder(rootCommand);
	}
}