namespace RGen.Application;

public enum ExitCode
{
	OK = 0,
	UnhandledException = -1,
	CommandExecutionException = -2,

	NoDataGenerated = -10,

	OutputFilePathError = -20,
	OutputFileWriteError = -21
}