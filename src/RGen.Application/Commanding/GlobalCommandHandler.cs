using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;


namespace RGen.Application.Commanding;

public abstract class GlobalCommandHandler : ICommandHandler
{
#region Global Options
	public bool NoColor { get; set; }
	public FileInfo? Output { get; set; }
#endregion Global Options

	public int Invoke(InvocationContext context) =>
		InvokeAsync(context).GetAwaiter().GetResult();

	public async Task<int> InvokeAsync(InvocationContext context)
	{
		try
		{
			return (int)await InvokeCoreAsync(context);
		}
		catch (Exception ex)
		{
			ConsoleHelper.PrintException(ex, "Error executing command");
			return (int)ExitCode.CommandExecutionException;
		}
	}

	protected abstract Task<ExitCode> InvokeCoreAsync(InvocationContext context);
}