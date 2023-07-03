using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;


namespace RGen.Logic.Integer;

public class GenerateIntegerHandler : GlobalCommandHandler
{
	protected override Task<int> InvokeCoreAsync(InvocationContext context)
	{
//TODO: Generate actual random integer
		Console.WriteLine(DateTime.Now.Millisecond);
		return Task.FromResult(0);
	}
}