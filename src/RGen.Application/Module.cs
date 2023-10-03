using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RGen.Application.Formatting;
using RGen.Application.Generating;
using RGen.Application.Rendering;
using RGen.Application.Writing;
using RGen.Domain;
using RGen.Domain.Formatting;
using RGen.Domain.Generating.Generators;
using RGen.Infrastructure.Rendering.Console;
using RGen.Infrastructure.Writing.Console;
using RGen.Infrastructure.Writing.TextFile;


namespace RGen.Application;


public static class Module
{
	public static IServiceCollection AddRgen(this IServiceCollection services)
	{
		services.AddSingleton<IntegerGenerator>();
		services.AddSingleton<IGeneratorService, GeneratorService>();

		services.AddSingleton(_ =>
			new GeneratorFactory()
				.Register<IntegerGeneratorOptions>(_ =>
					new IntegerGenerator()));

		services.AddSingleton(sp =>
			new FormatterFactory()
				.Register<IntegerFormatterOptions>(_ =>
					new IntegerFormatter(sp.GetRequiredService<ILogger<IntegerFormatter>>())));

		services.AddSingleton(_ =>
			new RendererFactory()
				.Register<ConsoleRendererOptions>(o => new ConsoleRenderer(o)));

		services.AddSingleton(sp =>
			new WriterFactory()
				.Register<ConsoleWriterOptions>(o => new ConsoleWriter(o))
				.Register<PlainTextFileWriterOptions>(o =>
					new PlainTextFileWriter(sp.GetRequiredService<ILogger<PlainTextFileWriter>>(), o)));

		return services;
	}
}