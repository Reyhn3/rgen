using System;
using System.CommandLine.Invocation;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RGen.Domain;
using RGen.Infrastructure;
using RGen.Infrastructure.Logging;


namespace RGen.Application.Commanding.Explain;

public class ExplainHandler : GlobalCommandHandler
{
	public ExplainHandler(ILogger<GlobalCommandHandler> logger)
		: base(logger)
	{}

	public int Code { get; set; }

	protected override Task<ExitCode> InvokeCoreAsync(InvocationContext context, CancellationToken cancellationToken)
	{
		try
		{
			if (Enum.IsDefined(typeof(ExitCode), Code))
			{
				var code = (ExitCode)Code;
				var description = GetEnumDescription(code);
				Logger.LogWarning("Exit code {ExitCode} is {ExitName} which means {ExitDescription}", Code, code, description);
				return Task.FromResult(ExitCode.OK);
			}

			if (Enum.IsDefined(typeof(ResultCode), Code))
			{
				var code = (ResultCode)Code;
				var description = GetEnumDescription(code);
				Logger.LogWarning("Exit code {ExitCode} is {ExitName} which means {ExitDescription}", Code, code, description);
				return Task.FromResult(ExitCode.OK);
			}

			Logger.LogError("The specified exit code {ExitCode} is not a recognized value", Code);
			return Task.FromResult(ExitCode.UserError);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Unable to explain code {ExitCode}", Code);
			LogHelper.PrintExceptionDetails(ex);
			return Task.FromResult(ExitCode.UnhandledCommandException);
		}
	}

	private static string GetEnumDescription<TEnum>(TEnum source)
	{
		var field = typeof(TEnum).GetField(source.ToString());
		var attributes = (DescriptionAttribute[])(field?.GetCustomAttributes(typeof(DescriptionAttribute), false) ?? Array.Empty<DescriptionAttribute>());
		return attributes is {Length: > 0} ? attributes[0].Description : source.ToString();
	}
}