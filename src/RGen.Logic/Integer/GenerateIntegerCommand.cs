using System.CommandLine;


namespace RGen.Logic.Integer;

public class GenerateIntegerCommand : Command
{
	public GenerateIntegerCommand()
		: base("int", "Generate integer numbers")
	{
		AddArgument(new Argument<int>("n", () => 1, "The number of values to generate"));
	}
}