using System;
using System.Threading;
using System.Threading.Tasks;


namespace RGen.Application.Writing;

public class ConsoleWriter : IWriter
{
	public Task WriteAsync(string values, CancellationToken cancellationToken)
	{
		Console.WriteLine(values);
		return Task.CompletedTask;
	}
}