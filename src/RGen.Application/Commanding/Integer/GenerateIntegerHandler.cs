using System;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using RGen.Application.Formatting;
using RGen.Application.Writing;
using RGen.Domain.Generators;


namespace RGen.Application.Commanding.Integer;

public class GenerateIntegerHandler : GlobalCommandHandler
{
	private readonly IIntegerGenerator _generator;
	private readonly IFormatter _formatter;
	private readonly IWriter _writer;

	public GenerateIntegerHandler(IIntegerGenerator generator, IFormatter formatter, IWriter writer)
	{
		_generator = generator ?? throw new ArgumentNullException(nameof(generator));
		_formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
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
		var formatted = _formatter.Format(sets, NoColor);
//TODO: Get CT from call-chain
		await _writer.WriteAsync(formatted, CancellationToken.None);

		return ExitCodes.OK;
	}
}