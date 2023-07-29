using System.CommandLine;


namespace RGen.Application.Commanding.Integer;

public class GenerateIntegerCommand : Command
{
	public GenerateIntegerCommand()
		: base("int", "Generate integer value(s)")
	{
		AddArgument(
			new Argument<int>(
					"n",
					() => 1,
					"The number of values to generate")
				.InValidRangeOnly(1, 1_000_000));

		AddOption(
			new Option<int>(
					"--set",
					() => 1,
					"The number of sets to generate")
				.InValidRangeOnly(1, ushort.MaxValue));
	}
}