using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RGen.Application.Commanding.Globals;
using RGen.Infrastructure;
using RGen.Infrastructure.Logging;


namespace RGen.Application.Commanding;

public abstract class GlobalCommandHandler : ICommandHandler
{
	protected GlobalCommandHandler(ILogger<GlobalCommandHandler> logger)
	{
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public ILogger<GlobalCommandHandler> Logger { get; }

#region Global Options
	public bool NoColor { get; set; }
	public FileInfo? Output { get; set; }
	public OutputFormat Format { get; set; }
#endregion Global Options

	public int Invoke(InvocationContext context) =>
		InvokeAsync(context).GetAwaiter().GetResult();

	public async Task<int> InvokeAsync(InvocationContext context)
	{
		try
		{
			return (int)await InvokeCoreAsync(context, context.GetCancellationToken());
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Unhandled exception when invoking command handler");
			LogHelper.PrintExceptionDetails(ex);
			return (int)ExitCode.UnhandledCommandException;
		}
	}

	protected abstract Task<ExitCode> InvokeCoreAsync(InvocationContext context, CancellationToken cancellationToken);
}