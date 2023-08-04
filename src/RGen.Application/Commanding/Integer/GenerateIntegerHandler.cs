using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using RGen.Application.Formatting;
using RGen.Application.Writing;
using RGen.Domain;
using RGen.Domain.Generating;
using RGen.Domain.Generating.Generators;
using RGen.Domain.Writing;
using RGen.Infrastructure.Formatting.Console;
using RGen.Infrastructure.Writing.Console;
using RGen.Infrastructure.Writing.TextFile;


namespace RGen.Application.Commanding.Integer;

public class GenerateIntegerHandler : GlobalCommandHandler
{
	private readonly IGeneratorService _generatorService;
	private readonly IGenerator _generator;
	private readonly IFormatterFactory _formatterFactory;
	private readonly IWriterFactory _writerFactory;

	public GenerateIntegerHandler(
		IGeneratorService generatorService,
		IntegerGenerator generator,
		IFormatterFactory formatterFactory,
		IWriterFactory writerFactory)
	{
		_generatorService = generatorService ?? throw new ArgumentNullException(nameof(generatorService));
		_generator = generator ?? throw new ArgumentNullException(nameof(generator));
		_formatterFactory = formatterFactory ?? throw new ArgumentNullException(nameof(formatterFactory));
		_writerFactory = writerFactory ?? throw new ArgumentNullException(nameof(writerFactory));
	}

	public int N { get; set; }
	public int Set { get; set; }

	protected override async Task<ExitCode> InvokeCoreAsync(InvocationContext context, CancellationToken cancellationToken)
	{
//TODO: #12: If more than x number of total elements, display a progress bar
//TODO: #11: If more than x number of total elements, run in parallel

		var formatter = _formatterFactory.Create(new ConsoleFormatterOptions(NoColor));
		var writers = CreateWriters();

		var result = await _generatorService.GenerateAsync(
			_generator,
			formatter,
			writers,
			N,
			Set,
			cancellationToken);

		return result.ToExitCode();
	}

	private IEnumerable<IWriter> CreateWriters()
	{
//TODO: #4: Don't use this if verbosity is quiet
		yield return _writerFactory.Create(new ConsoleWriterOptions());

		if (Output != null)
			yield return _writerFactory.Create(new PlainTextFileWriterOptions(Output));
	}
}