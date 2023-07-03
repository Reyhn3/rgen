using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;


namespace RGen.Logic;

public abstract class GlobalCommandHandler : ICommandHandler
{
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
			CliHelper.PrintException(ex, "Error executing command");
			return default;
		}
	}

	protected abstract Task<int> InvokeCoreAsync(InvocationContext context);
}