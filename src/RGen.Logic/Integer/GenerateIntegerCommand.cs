using System.CommandLine;


namespace RGen.Logic.Integer;

public class GenerateIntegerCommand : Command
{
	public GenerateIntegerCommand()
		: base("int", "Generate integer value(s)")
	{
		AddArgument(new Argument<int>("n", () => 1, "The number of values to generate"));

		AddOption(new Option<int>("--set", () => 1, "The number of sets to generate"));
	}
}