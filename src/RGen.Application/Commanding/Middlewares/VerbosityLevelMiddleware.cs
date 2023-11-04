using System.CommandLine.Invocation;
using RGen.Application.Commanding.Globals;
using RGen.Infrastructure.Logging;


namespace RGen.Application.Commanding.Middlewares;

public static class VerbosityLevelMiddleware
{
	public static InvocationMiddleware Instance =
		async (context, next) =>
			{
				VerbosityLevel level;

				try
				{
					if (context.ParseResult.FindResultFor(GlobalVerbosityOption.Loud) != null)
						level = VerbosityLevel.Verbose;
					else if (context.ParseResult.FindResultFor(GlobalVerbosityOption.Quiet) != null)
						level = VerbosityLevel.Quiet;
					else
						level = context.ParseResult.GetValueForOption(GlobalVerbosityOption.Verbosity);
				}
				catch
				{
					level = VerbosityLevel.Normal;
				}

				LogHelper.Switch.MinimumLevel = LogHelper.ToLogLevel(level);

				await next(context);
			};
}