using System.ComponentModel;


namespace RGen.Application;

public enum ExitCode
{
	[Description("Command executed successfully")]
	OK = 0,


#region Application-level errors
	[Description("Running multiple instances is not allowed")]
	MultipleInstances = -1,

	[Description("Command was cancelled")]
	Cancelled = -2,

	[Description("User input was invalid")]
	UserError = -3,
#endregion Application-level errors

#region General errors
	[Description("General: An exception was unhandled")]
	UnhandledException = -10,

	[Description("General: A command exception was unhandled")]
	UnhandledCommandException = -11,
#endregion General errors
}