using System;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using RGen.Application.Formatting;
using RGen.Application.Formatting.Console;
using RGen.Application.Writing;
using RGen.Application.Writing.Console;
using RGen.Domain.Generators;


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

	protected override async Task<int> InvokeCoreAsync(InvocationContext context)
	{
//TODO: Refactor to be a chained process, e.g. generate -> format -> output

//TODO: Validate boundaries (Set should be >= 1)
//TODO: Validate boundaries (N should be >= 1)

		var sets = _generator.Set(N, Set);

		var formatter = _formatterFactory.Create(new ConsoleFormatterOptions(NoColor));
		var formatted = formatter.Format(sets);

//TODO: Get CT from call-chain
		var writer = _writerFactory.Create(new ConsoleWriterOptions());
		await writer.WriteAsync(formatted, CancellationToken.None);

		return ExitCodes.OK;
	}
}