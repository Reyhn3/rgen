using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RGen.Application.Commanding;
using RGen.Application.Commanding.Integer;
using RGen.Application.Formatting;
using RGen.Application.Writing;
using RGen.Domain;
using RGen.Domain.Generating;
using RGen.Domain.Generating.Generators;
using RGen.Infrastructure;
using RGen.Infrastructure.Formatting.Console;
using RGen.Infrastructure.Writing.Console;
using RGen.Infrastructure.Writing.TextFile;


namespace RGen;

internal static class Startup
{
	public static Parser BuildParser() =>
		BuildCommandLine()
			.UseDefaults()
			.UseHelp(help => help
				.HelpBuilder.CustomizeLayout(_ =>
					HelpBuilder.Default
						.GetLayout()
						.Skip(1)
						.Prepend(_ =>
							Splash.Render())))
			.UseHost(
				Host.CreateDefaultBuilder,
				host => host
					.ConfigureLogging(builder => builder.ClearProviders())
					.UseConsoleLifetime()
					.UseContentRoot(AppContext.BaseDirectory)
					.ConfigureServices(services => services
						.AddSingleton<IGeneratorService, GeneratorService>()
						.AddSingleton<IntegerGenerator>()
						.AddSingleton(_ =>
							new FormatterFactory()
								.Register<ConsoleFormatterOptions>(o => new ConsoleFormatter(o)))
						.AddSingleton(_ =>
							new WriterFactory()
								.Register<ConsoleWriterOptions>(o => new ConsoleWriter(o))
								.Register<PlainTextFileWriterOptions>(o => new PlainTextFileWriter(o))))
					.UseCommandHandler<GenerateIntegerCommand, GenerateIntegerHandler>())
			.Build();

	private static CommandLineBuilder BuildCommandLine()
	{
		var rootCommand = new RootCommand(ConsoleHelper.GetProductName(typeof(Program).Assembly))
			{
				new GenerateIntegerCommand()
			};

		rootCommand.AddGlobalOption(GlobalColorOption.Create());
		rootCommand.AddGlobalOption(GlobalOutputOption.Create());

		return new CommandLineBuilder(rootCommand);
	}
}