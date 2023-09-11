using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RGen.Application.Formatting;
using RGen.Application.Rendering;
using RGen.Application.Writing;
using RGen.Domain;
using RGen.Domain.Formatting;
using RGen.Domain.Generating;
using RGen.Domain.Generating.Generators;
using RGen.Domain.Writing;
using RGen.Infrastructure.Rendering.Console;
using RGen.Infrastructure.Writing.Console;
using RGen.Infrastructure.Writing.TextFile;


namespace RGen.Application.Commanding.Integer;


public class GenerateIntegerHandler : GlobalCommandHandler
{
	private readonly IGeneratorService _generatorService;
	private readonly IFormatterFactory _formatterFactory;
	private readonly IGenerator _generator;
	private readonly IRendererFactory _rendererFactory;
	private readonly IWriterFactory _writerFactory;

	public GenerateIntegerHandler(
		ILogger<GenerateIntegerHandler> logger,
		IntegerGenerator generator,
		IGeneratorService generatorService,
		IFormatterFactory formatterFactory,
		IRendererFactory rendererFactory,
		IWriterFactory writerFactory)
		: base(logger)
	{
		_generatorService = generatorService ?? throw new ArgumentNullException(nameof(generatorService));
		_formatterFactory = formatterFactory ?? throw new ArgumentNullException(nameof(formatterFactory));
		_generator = generator ?? throw new ArgumentNullException(nameof(generator));
		_rendererFactory = rendererFactory ?? throw new ArgumentNullException(nameof(rendererFactory));
		_writerFactory = writerFactory ?? throw new ArgumentNullException(nameof(writerFactory));
	}

	public int N { get; set; }
	public int Set { get; set; }
	public int? Length { get; set; }
	public int? Min { get; set; }
	public int? Max { get; set; }
	public IntegerBase Base { get; set; }

	protected override async Task<ExitCode> InvokeCoreAsync(InvocationContext context, CancellationToken cancellationToken)
	{
//TODO: #12: If more than x number of total elements, display a progress bar
//TODO: #11: If more than x number of total elements, run in parallel

		var formatter = _formatterFactory.Create(new IntegerFormatterOptions(Base));
		var renderer = _rendererFactory.Create(new ConsoleRendererOptions(NoColor));
		var writers = CreateWriters();

		var result = await _generatorService.GenerateAsync(
			_generator,
			formatter,
			renderer,
			writers,
			N,
			Set,
			Length,
			Min,
			Max,
			cancellationToken);

		return result.ToExitCode();
	}

	private IEnumerable<IWriter> CreateWriters()
	{
		yield return _writerFactory.Create(new ConsoleWriterOptions());

		if (Output != null)
			yield return _writerFactory.Create(new PlainTextFileWriterOptions(Output));
	}
}