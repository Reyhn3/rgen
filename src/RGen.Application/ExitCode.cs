namespace RGen.Application;

public enum ExitCode
{
	OK = 0,
	UnhandledException = -1,
	UnhandledCommandException = -2,

	NoDataGenerated = -100
}