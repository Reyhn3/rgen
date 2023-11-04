using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RGen.Application.Commanding.Globals;
using RGen.Application.Formatting;
using RGen.Application.Generating;
using RGen.Application.Rendering;
using RGen.Application.Writing;
using RGen.Domain;
using RGen.Domain.Formatting;
using RGen.Domain.Generating.Generators;
using RGen.Domain.Writing;
using RGen.Infrastructure.Rendering.Console;
using RGen.Infrastructure.Writing.Console;
using RGen.Infrastructure.Writing.TextFile;


namespace RGen.Application.Commanding.Integer;


public class GenerateIntegerHandler : GlobalCommandHandler
{
	private readonly IGeneratorService _generatorService;
	private readonly IGeneratorFactory _generatorFactory;
	private readonly IFormatterFactory _formatterFactory;
	private readonly IRendererFactory _rendererFactory;
	private readonly IWriterFactory _writerFactory;

	public GenerateIntegerHandler(
		ILogger<GenerateIntegerHandler> logger,
		IGeneratorService generatorService,
		IGeneratorFactory generatorFactory,
		IFormatterFactory formatterFactory,
		IRendererFactory rendererFactory,
		IWriterFactory writerFactory)
		: base(logger)
	{
		_generatorService = generatorService ?? throw new ArgumentNullException(nameof(generatorService));
		_generatorFactory = generatorFactory ?? throw new ArgumentNullException(nameof(generatorFactory));
		_formatterFactory = formatterFactory ?? throw new ArgumentNullException(nameof(formatterFactory));
		_rendererFactory = rendererFactory ?? throw new ArgumentNullException(nameof(rendererFactory));
		_writerFactory = writerFactory ?? throw new ArgumentNullException(nameof(writerFactory));
	}

	public int N { get; set; }
	public int S { get; set; }
	public int? Length { get; set; }
	public ulong? Min { get; set; }
	public ulong? Max { get; set; }
	public IntegerBase Base { get; set; }

	protected override async Task<ExitCode> InvokeCoreAsync(InvocationContext context, CancellationToken cancellationToken)
	{
//TODO: #12: If more than x number of total elements, display a progress bar
//TODO: #11: If more than x number of total elements, run in parallel

		var generator = _generatorFactory.Create(new IntegerGeneratorOptions(Length, Min, Max));
		var formatter = _formatterFactory.Create(new IntegerFormatterOptions(Base));
		var renderer = _rendererFactory.Create(new ConsoleRendererOptions(NoColor));
		var writers = CreateWriters();

		var result = await _generatorService.GenerateAsync(
			generator,
			formatter,
			renderer,
			writers,
			N,
			S,
			new IntegerGeneratorOptions(Length, Min, Max),
			new IntegerFormatterOptions(Base),
			new ConsoleRendererOptions(NoColor),
			cancellationToken);

		return result.ToExitCode();
	}

	private IEnumerable<IWriter> CreateWriters()
	{
		yield return _writerFactory.Create(new ConsoleWriterOptions());

		if (Output == null)
			yield break;

		var formatToUse = Enum.IsDefined(Format) ? Format : OutputFormat.Text;
		var formatExtension = ConvertOutputFormatToFileExtension(formatToUse);

		yield return _writerFactory.Create(new TextFileWriterOptions(Output, formatExtension));
	}

	private static string ConvertOutputFormatToFileExtension(OutputFormat format) =>
		format switch
			{
				OutputFormat.Text => "txt",
				_                 => throw new ArgumentOutOfRangeException(nameof(format), format, null)
			};
}