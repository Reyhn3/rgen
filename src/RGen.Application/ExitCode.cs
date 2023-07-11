namespace RGen.Application;

public enum ExitCode
{
	OK = 0,
	UnhandledException = -1,
	CommandExecutionException = -2,

	OutputFilePathError = -10,
	OutputFileWriteError = -11
}