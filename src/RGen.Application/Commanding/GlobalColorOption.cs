using System.CommandLine;


namespace RGen.Application.Commanding;

public static class GlobalColorOption
{
	public static Option<bool> Create() => new(
			new[]
				{
					"--no-color"
				},
			() => false,
			"Disable console output colors")
			{
				IsRequired = false
			};
}