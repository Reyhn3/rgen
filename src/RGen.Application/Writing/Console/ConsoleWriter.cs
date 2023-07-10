using System.Threading;
using System.Threading.Tasks;
using StdOut = System.Console;


namespace RGen.Application.Writing.Console;

public class ConsoleWriter : IWriter
{
	private readonly ConsoleWriterOptions _options;

	public ConsoleWriter()
		: this(new ConsoleWriterOptions())
	{}

	public ConsoleWriter(ConsoleWriterOptions options)
	{
		_options = options;
	}

	public Task WriteAsync(string values, CancellationToken cancellationToken)
	{
		StdOut.WriteLine(values);
		return Task.CompletedTask;
	}
}