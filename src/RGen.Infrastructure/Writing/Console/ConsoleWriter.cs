using System.Threading;
using System.Threading.Tasks;
using RGen.Domain;
using RGen.Domain.Rendering;
using RGen.Domain.Writing;
using RGen.Infrastructure.Logging;
using StdOut = System.Console;


namespace RGen.Infrastructure.Writing.Console;

public class ConsoleWriter : IWriter
{
	private readonly ConsoleWriterOptions _options;

	public ConsoleWriter(ConsoleWriterOptions options)
	{
		_options = options;
	}

	public Task<IResult> WriteAsync(RenderContext context, CancellationToken cancellationToken)
	{
		if (LogHelper.IsQuiet)
			return Task.FromResult(Result.OK);

		StdOut.WriteLine(context.Rendered);
		return Task.FromResult(Result.OK);
	}
}