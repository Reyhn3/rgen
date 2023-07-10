using System;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using RGen.Application.Formatting;
using RGen.Application.Formatting.Console;
using RGen.Application.Writing;
using RGen.Domain.Generators;


namespace RGen.Application.Commanding.Integer;

public class GenerateIntegerHandler : GlobalCommandHandler
{
	private readonly IIntegerGenerator _generator;
	private readonly IFormatterFactory _formatterFactory;
	private readonly IWriter _writer;

	public GenerateIntegerHandler(IIntegerGenerator generator, IFormatterFactory formatterFactory, IWriter writer)
	{
		_generator = generator ?? throw new ArgumentNullException(nameof(generator));
		_formatterFactory = formatterFactory ?? throw new ArgumentNullException(nameof(formatterFactory));
		_writer = writer ?? throw new ArgumentNullException(nameof(writer));
	}

	public int N { get; set; }
	public int Set { get; set; }

	protected override async Task<int> InvokeCoreAsync(InvocationContext context)
	{
//TODO: Refactor to be a chained process, e.g. generate -> format -> output

//TODO: Validate boundaries (Set should be >= 1)
//TODO: Validate boundaries (N should be >= 1)

		var sets = _generator.Set(N, Set);

		var options = new ConsoleFormatterOptions(NoColor);
		var formatter = _formatterFactory.Create(options);
		var formatted = formatter.Format(sets);

//TODO: Get CT from call-chain
		await _writer.WriteAsync(formatted, CancellationToken.None);

		return ExitCodes.OK;
	}
}