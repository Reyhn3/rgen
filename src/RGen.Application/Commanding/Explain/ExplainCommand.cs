using System.CommandLine;


namespace RGen.Application.Commanding.Explain;

public class ExplainCommand : Command
{
	public ExplainCommand()
		: base("explain", "Interpret exit codes from this application")
	{
		AddArgument(new Argument<int>("code", "The exit code to explain"));
	}
}