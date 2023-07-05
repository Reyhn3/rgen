using System;
using System.Threading;
using System.Threading.Tasks;


namespace RGen.Logic.Output;

public class ConsoleOutput : IOutput
{
	public Task WriteAsync(string values, CancellationToken cancellationToken)
	{
		Console.WriteLine(values);
		return Task.CompletedTask;
	}
}