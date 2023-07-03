using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;


namespace RGen.Logic.Integer;

public class GenerateIntegerHandler : GlobalCommandHandler
{
	private readonly IIntegerGenerator _generator;

	public GenerateIntegerHandler(IIntegerGenerator generator)
	{
		_generator = generator ?? throw new ArgumentNullException(nameof(generator));
	}

	public int N { get; set; }
	public int Set { get; set; }

	protected override Task<int> InvokeCoreAsync(InvocationContext context)
	{
//TODO: Validate boundaries (Set should be >= 1)
		if (Set == 1)
		{
			var numbers = _generator.Multiple(N);
			foreach (var number in numbers)
				Console.WriteLine(number);
		}
		else
		{
			var sets = _generator.Set(N, Set);
			foreach (var set in sets)
//TODO: Extract formatting to separate class
				Console.WriteLine("[{0}]", string.Join(", ", set));
		}

		return Task.FromResult(0);
	}
}