using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RGen.Logic;
using RGen.Logic.Integer;


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