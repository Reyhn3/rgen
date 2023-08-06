using System.CommandLine;
using RGen.Infrastructure.Logging;


namespace RGen.Application.Commanding.Globals;

public static class GlobalVerbosityOption
{
//TEST: No argument
//TEST: --verbosity
//TEST: --verbosity minimal
//TEST: --verbosity minimal -q
//TEST: --verbosity minimal -v
//TEST: -v
//TEST: -q
//TEST: -v -q
//TEST: -q -v

	public static Option<VerbosityLevel> Verbosity =
		new("--verbosity", () => VerbosityLevel.Normal, "Sets level of feedback")
			{
				Arity = ArgumentArity.ExactlyOne
			};

	public static Option<bool> Loud =
		new("-v", "Enable full logging (shortcut for '--verbosity verbose')");

	public static Option<bool> Quiet =
		new("-q", "Disable logging (shortcut for '--verbosity quiet')");
}