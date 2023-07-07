using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;


namespace RGen.Application.Commanding;

public abstract class GlobalCommandHandler : ICommandHandler
{
#region Global Options
	public bool NoColor { get; set; }
#endregion Global Options

	public int Invoke(InvocationContext context) =>
		InvokeAsync(context).GetAwaiter().GetResult();

	public async Task<int> InvokeAsync(InvocationContext context)
	{
		try
		{
			return await InvokeCoreAsync(context).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			ConsoleHelper.PrintException(ex, "Error executing command");
			return ExitCodes.CommandExecutionException;
		}
	}

	protected abstract Task<int> InvokeCoreAsync(InvocationContext context);
}