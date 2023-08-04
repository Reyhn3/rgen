using RGen.Domain;


namespace RGen.Application;

internal static class ExitCodeExtensions
{
//TODO: Maybe do a better mapping...
	public static ExitCode ToExitCode(this IResult result) =>
		(ExitCode)result.Code;
}