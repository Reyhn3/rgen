using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RGen.Application.Formatting;
using RGen.Application.Writing;
using RGen.Domain;
using RGen.Domain.Generating.Generators;
using RGen.Infrastructure.Writing.Console;
using RGen.Infrastructure.Writing.TextFile;


namespace RGen.Application.Commanding.Integer;

public class GenerateIntegerHandler : GlobalCommandHandler
{
	private readonly IIntegerGenerator _generator;
	private readonly IFormatterFactory _formatterFactory;
	private readonly IWriterFactory _writerFactory;

	public GenerateIntegerHandler(
		IIntegerGenerator generator, 
		IFormatterFactory formatterFactory, 
		IWriterFactory writerFactory)
	{
		_generator = generator ?? throw new ArgumentNullException(nameof(generator));
		_formatterFactory = formatterFactory ?? throw new ArgumentNullException(nameof(formatterFactory));
		_writerFactory = writerFactory ?? throw new ArgumentNullException(nameof(writerFactory));
	}

	public int N { get; set; }
	public int Set { get; set; }

	protected override async Task<ExitCode> InvokeCoreAsync(InvocationContext context, CancellationToken cancellationToken)
	{
//TODO: Refactor to be a chained process, e.g. generate -> format -> output

//TODO: #12: If more than x number of total elements, display a progress bar
//TODO: #11: If more than x number of total elements, run in parallel
		var sets = _generator.Set(N, Set);

		var formatter = _formatterFactory.Create(new RGen.Infrastructure.Formatting.Console.ConsoleFormatterOptions(NoColor));
		var formatted = formatter.Format(sets);
		if (formatted.IsEmpty)
			return ExitCode.NoDataGenerated;

		var consoleResult = await WriteToConsole(formatted.Formatted, cancellationToken);
		if (!consoleResult.IsSuccessful)
			return consoleResult.ToExitCode();

		var outputResult = await WriteToOutput(formatted.Raw, Output, cancellationToken);
		if (!outputResult.IsSuccessful)
			return outputResult.ToExitCode();

		return ExitCode.OK;
	}

	private async Task<IResult> WriteToConsole(string content, CancellationToken cancellationToken)
	{
		var writer = _writerFactory.Create(new ConsoleWriterOptions());
		var writeResult = await writer.WriteAsync(content, cancellationToken);
		return writeResult;
	}

	private async Task<IResult> WriteToOutput(string content, FileInfo? output, CancellationToken cancellationToken)
	{
		if (output == null)
			return Result.OK;

		var writer = _writerFactory.Create(new PlainTextFileWriterOptions(output));
		var writeResult = await writer.WriteAsync(content, cancellationToken);
		return writeResult;
	}
}