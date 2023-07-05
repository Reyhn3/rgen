using System;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using RGen.Logic.Formatting;
using RGen.Logic.Output;


namespace RGen.Logic.Integer;

public class GenerateIntegerHandler : GlobalCommandHandler
{
	private readonly IIntegerGenerator _generator;
	private readonly IFormatter _formatter;
	private readonly IOutput _output;

	public GenerateIntegerHandler(IIntegerGenerator generator, IFormatter formatter, IOutput output)
	{
		_generator = generator ?? throw new ArgumentNullException(nameof(generator));
		_formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
		_output = output ?? throw new ArgumentNullException(nameof(output));
	}

	public int N { get; set; }
	public int Set { get; set; }

	protected override async Task<int> InvokeCoreAsync(InvocationContext context)
	{
//TODO: Refactor to be a chained process, e.g. generate -> format -> output

//TODO: Validate boundaries (Set should be >= 1)
//TODO: Validate boundaries (N should be >= 1)

		var sets = _generator.Set(N, Set);
		var formatted = _formatter.Format(sets);
//TODO: Get CT from call-chain
		await _output.WriteAsync(formatted, CancellationToken.None);

		return ExitCodes.OK;
	}
}