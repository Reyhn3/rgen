namespace RGen.Application;

public enum ExitCode
{
	OK = 0,

	// Codes in the range -99 to -1 are reserved for
	// general application errors.

	MultipleInstances = -1,
	UnhandledException = -2,
	UnhandledCommandException = -3,

	// Codes below -99 are application specific errors.
	NoDataGenerated = -100
}